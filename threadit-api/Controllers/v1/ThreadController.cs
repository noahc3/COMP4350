using System.Net.Cache;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;
using ThreaditAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/thread")]
    public class ThreadController : ControllerBase {
        
        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThread([FromRoute] string threadId, [FromServices] ThreadService threadService) {            
            Models.ThreadFull? threadFull = await threadService.GetThreadFullAsync(threadId);
            return Ok(threadFull);
        }

        [HttpPost("create")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostThread([FromBody] PostThreadRequest request, [FromServices] ThreadService threadService) {            
            UserDTO? userDTO = Request.HttpContext.GetUser();
            
            Models.Thread thread = new Models.Thread{
                Title = request.Title,
                Content = request.Content,
                Topic = request.Topic,
                OwnerId = userDTO.Id, 
                SpoolId = request.SpoolId
            };

            try {
                thread = await threadService.InsertThreadAsync(thread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
            return Ok(thread);
        }

        [HttpGet("threads/{sortType?}")]
        public async Task<IActionResult> GetThreads([FromServices] ThreadService threadService, [FromRoute] string? sortType = null, [FromQuery(Name = "q")] string? searchWord = null, [FromQuery(Name = "skip")] int? skip = null, [FromQuery(Name = "spoolId")] string? spoolId = null) {
            return Ok(await threadService.GetThreadsAsync(sortType, searchWord, skip, spoolId));
        }

        [HttpPost("edit")]
        [AuthenticationRequired]
        public async Task<IActionResult> EditThread([FromServices] ThreadService threadService, [FromBody] Models.Thread thread) {
            if (thread == null || String.IsNullOrWhiteSpace(thread.Id)) {
                return BadRequest("Thread data is invalid.");
            }

            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb = await threadService.GetThreadAsync(thread.Id);

            if (threadFromDb.OwnerId != profile.Id) {
                return Unauthorized();
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(thread);
            return Ok(updatedThread);
        }

        [HttpDelete("{threadId}")]
        [AuthenticationRequired]
        public async Task<IActionResult> DeleteThread([FromRoute] string threadId, [FromServices] ThreadService threadService) {
            UserDTO profile = Request.HttpContext.GetUser();

            await threadService.DeleteThreadAsync(threadId, profile.Id);
            return Ok();
        }

        [HttpPost("stitch")]
        [AuthenticationRequired]
        public async Task<IActionResult> StitchThread([FromBody] string threadId, [FromServices] ThreadService threadService) {
            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb;
            try {
                threadFromDb = await threadService.GetThreadAsync(threadId);
            } catch (Exception) {
                return BadRequest("Thread does not exist.");
            }

            if(threadFromDb.Stitches.Contains(profile.Id))
            {
                threadFromDb.Stitches.Remove(profile.Id);
            }
            else
            {
                threadFromDb.Stitches.Add(profile.Id);
            }

            if(threadFromDb.Rips.Contains(profile.Id))
            {
                threadFromDb.Rips.Remove(profile.Id);
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
            return Ok(updatedThread);
        }

        [HttpPost("rip")]
        [AuthenticationRequired]
        public async Task<IActionResult> RipThread([FromBody] string threadId, [FromServices] ThreadService threadService) {
            UserDTO profile = Request.HttpContext.GetUser();

            Models.Thread threadFromDb;
            try {
                threadFromDb = await threadService.GetThreadAsync(threadId);
            } catch (Exception) {
                return BadRequest("Thread does not exist.");
            }

            if(threadFromDb.Rips.Contains(profile.Id))
            {
                threadFromDb.Rips.Remove(profile.Id);
            }
            else
            {
                threadFromDb.Rips.Add(profile.Id);
            }

            if(threadFromDb.Stitches.Contains(profile.Id))
            {
                threadFromDb.Stitches.Remove(profile.Id);
            }

            Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
            return Ok(updatedThread);
        }
    }
}
