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

            thread = await threadService.InsertThreadAsync(thread);
            return Ok(thread);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllThreads([FromServices] ThreadService threadService) {
            Models.ThreadFull[] threads = await threadService.GetAllThreadsAsync();
            return Ok(threads);
        }

        [HttpGet("all/{sortType}")]
        public async Task<IActionResult> GetAllThreadsFiltered([FromRoute] string sortType, [FromServices] ThreadService threadService, [FromServices] FilterService filterService, [FromQuery(Name = "q")] string searchWord = "") {
            Models.ThreadFull[] threads = await threadService.GetAllThreadsAsync();
            if(searchWord != "")
            {
                threads = filterService.SearchThreads(threads.ToArray(), searchWord).ToArray();
            }
            if(sortType != "")
            {
                threads = filterService.SortThreads(threads.ToArray(), sortType).ToArray();
            }
            return Ok(threads);
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
            if (String.IsNullOrWhiteSpace(threadId)) {
                return BadRequest("Thread ID is invalid.");
            }

            UserDTO profile = Request.HttpContext.GetUser();

            await threadService.DeleteThreadAsync(threadId, profile.Id);
            return Ok();
        }

        [HttpPost("stitch")]
        [AuthenticationRequired]
        public async Task<IActionResult> StitchThread([FromBody] string threadId, [FromServices] ThreadService threadService) {
            if (String.IsNullOrWhiteSpace(threadId)) {
                return BadRequest("Thread data is invalid.");
            }

            UserDTO? profile = Request.HttpContext.GetUser();
            if (profile == null) {
                return Unauthorized();
            }

            Models.Thread? threadFromDb = await threadService.GetThreadAsync(threadId);
            if (threadFromDb == null) {
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

            try {
                Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
                return Ok(updatedThread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("rip")]
        [AuthenticationRequired]
        public async Task<IActionResult> RipThread([FromBody] string threadId, [FromServices] ThreadService threadService) {
            if (String.IsNullOrWhiteSpace(threadId)) {
                return BadRequest("Thread data is invalid.");
            }

            UserDTO? profile = Request.HttpContext.GetUser();
            if (profile == null) {
                return Unauthorized();
            }

            Models.Thread? threadFromDb = await threadService.GetThreadAsync(threadId);
            if (threadFromDb == null) {
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

            try {
                Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
                return Ok(updatedThread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
