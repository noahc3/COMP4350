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

            if (userDTO == null) {
                return Unauthorized();
            }
            
            Models.Thread thread = new Models.Thread{
                Title = request.Title,
                Content = request.Content,
                Topic = request.Topic,
                OwnerId = userDTO.Id, 
                SpoolId = request.SpoolId
            };

            try {
                thread = await threadService.InsertThreadAsync(thread);
                return Ok(thread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }   
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllThreads([FromServices] ThreadService threadService) {
            Models.ThreadFull[] threads = await threadService.GetAllThreadsAsync();
            return Ok(threads);
        }

        [HttpPost("edit")]
        [AuthenticationRequired]
        public async Task<IActionResult> EditThread([FromServices] ThreadService threadService, [FromBody] Models.Thread thread) {
            if (thread == null || String.IsNullOrWhiteSpace(thread.Id)) {
                return BadRequest("Thread data is invalid.");
            }

            UserDTO? profile = Request.HttpContext.GetUser();
            if (profile == null) {
                return Unauthorized();
            }

            Models.Thread? threadFromDb = await threadService.GetThreadAsync(thread.Id);
            if (threadFromDb == null) {
                return BadRequest("Thread does not exist.");
            }

            if (threadFromDb.OwnerId != profile.Id) {
                return Unauthorized();
            }

            try {
                Models.Thread? updatedThread = await threadService.UpdateThreadAsync(thread);
                return Ok(updatedThread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{threadId}")]
        [AuthenticationRequired]
        public async Task<IActionResult> DeleteThread([FromRoute] string threadId, [FromServices] ThreadService threadService) {
            if (String.IsNullOrWhiteSpace(threadId)) {
                return BadRequest("Thread ID is invalid.");
            }

            UserDTO? profile = Request.HttpContext.GetUser();
            if (profile == null) {
                return Unauthorized();
            }

            Models.Thread? threadFromDb = await threadService.GetThreadAsync(threadId);
            if (threadFromDb == null) {
                return BadRequest("Thread does not exist.");
            }

            if (threadFromDb.OwnerId != profile.Id) {
                return Unauthorized();
            }

            try {
                await threadService.DeleteThreadAsync(threadId);
                return Ok();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("stitch")]
        [AuthenticationRequired]
        public async Task<IActionResult> StitchThread([FromRoute] string threadId, [FromServices] ThreadService threadService) {
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
                return BadRequest($"User {profile.Id} already stitched thread.");
            }

            threadFromDb.Stitches.Add(profile.Id);

            try {
                Models.Thread? updatedThread = await threadService.UpdateThreadAsync(threadFromDb);
                return Ok(updatedThread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
