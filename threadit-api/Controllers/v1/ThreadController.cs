using System.Net;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using ThreaditAPI.Constants;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;
using ThreaditAPI.Services;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/thread")]
    public class ThreadController : ControllerBase
    {

        private static async Task ValidateImageUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res;

                try
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Head, url);
                    res = await client.SendAsync(msg);
                }
                catch (Exception)
                {
                    throw new Exception("Could not find an image at the specified URL. Please make sure the URL is a direct link to an image.");
                }

                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Website is offline or does not allow embedding this image. Consider using a link post to share this image.");
                }
                else if (!(res.Content.Headers.ContentType?.MediaType?.StartsWith("image/") ?? false))
                {
                    throw new Exception("Could not find an image at the specified URL. Please make sure the URL is a direct link to an image.");
                }
                else
                {
                    int length = 0;
                    if (res.Content.Headers.ContentLength.HasValue)
                    {
                        length = (int)res.Content.Headers.ContentLength.Value;
                    }

                    if (length == 0 || length > 5000000)
                    {
                        throw new Exception("For a smooth user experience, images cannot be larger than 5MB. Use a link post to share larger images.");
                    }
                }
            }
        }

        private static bool IsValidUrl(string url)
        {
            Uri? uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && uriResult != null
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThread([FromRoute] string threadId, [FromServices] ThreadService threadService)
        {
            Models.ThreadFull? threadFull = await threadService.GetThreadFullAsync(threadId);
            return Ok(threadFull);
        }

        [HttpPost("create")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostThread([FromBody] PostThreadRequest request, [FromServices] ThreadService threadService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            try
            {
                if (request.ThreadType == ThreadTypes.IMAGE)
                {
                    if (!IsValidUrl(request.Content)) {
                        return BadRequest("Could not find an image at the specified URL. Please make sure the URL is a direct link to an image.");
                    }
                    
                    await ValidateImageUrl(request.Content);
                }
                else if (request.ThreadType == ThreadTypes.LINK && !IsValidUrl(request.Content))
                {
                    return BadRequest("Could not find a page at the specified URL. Please make sure the URL is valid.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            Models.Thread thread = new Models.Thread
            {
                Title = request.Title,
                Content = request.Content,
                Topic = request.Topic,
                OwnerId = userDTO.Id,
                SpoolId = request.SpoolId,
                ThreadType = request.ThreadType
            };

            try
            {
                thread = await threadService.InsertThreadAsync(thread);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(thread);
        }

        [HttpGet("threads/{sortType?}")]
        public async Task<IActionResult> GetThreads([FromServices] ThreadService threadService, [FromRoute] string? sortType = null, [FromQuery(Name = "q")] string? searchWord = null, [FromQuery(Name = "skip")] int? skip = null, [FromQuery(Name = "spoolId")] string? spoolId = null)
        {
            return Ok(await threadService.GetThreadsAsync(sortType, searchWord, skip, spoolId));
        }

        [HttpPost("edit")]
        [AuthenticationRequired]
        public async Task<IActionResult> EditThread([FromServices] ThreadService threadService, [FromBody] Models.Thread thread)
        {
            if (thread == null || String.IsNullOrWhiteSpace(thread.Id))
            {
                return BadRequest("Thread data is invalid.");
            }

            if (thread.ThreadType != ThreadTypes.TEXT)
            {
                return BadRequest("Only text posts can be edited.");
            }

            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb = await threadService.GetThreadAsync(thread.Id);

            if (threadFromDb.OwnerId != profile.Id)
            {
                return Unauthorized();
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(thread);
            return Ok(updatedThread);
        }

        [HttpDelete("{threadId}")]
        [AuthenticationRequired]
        public async Task<IActionResult> DeleteThread([FromRoute] string threadId, [FromServices] ThreadService threadService)
        {
            UserDTO profile = Request.HttpContext.GetUser();

            await threadService.DeleteThreadAsync(threadId, profile.Id);
            return Ok();
        }

        [HttpPost("stitch")]
        [AuthenticationRequired]
        public async Task<IActionResult> StitchThread([FromBody] string threadId, [FromServices] ThreadService threadService)
        {
            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb;
            try
            {
                threadFromDb = await threadService.GetThreadAsync(threadId);
            }
            catch (Exception)
            {
                return BadRequest("Thread does not exist.");
            }

            if (threadFromDb.Stitches.Contains(profile.Id))
            {
                threadFromDb.Stitches.Remove(profile.Id);
            }
            else
            {
                threadFromDb.Stitches.Add(profile.Id);
            }

            if (threadFromDb.Rips.Contains(profile.Id))
            {
                threadFromDb.Rips.Remove(profile.Id);
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
            return Ok(updatedThread);
        }

        [HttpPost("rip")]
        [AuthenticationRequired]
        public async Task<IActionResult> RipThread([FromBody] string threadId, [FromServices] ThreadService threadService)
        {
            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb;
            try
            {
                threadFromDb = await threadService.GetThreadAsync(threadId);
            }
            catch (Exception)
            {
                return BadRequest("Thread does not exist.");
            }

            if (threadFromDb.Rips.Contains(profile.Id))
            {
                threadFromDb.Rips.Remove(profile.Id);
            }
            else
            {
                threadFromDb.Rips.Add(profile.Id);
            }

            if (threadFromDb.Stitches.Contains(profile.Id))
            {
                threadFromDb.Stitches.Remove(profile.Id);
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
            return Ok(updatedThread);
        }
    }
}
