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
    [Route("v1/spool")]
    public class SpoolController : ControllerBase {
        [HttpGet("threads/{spoolName}")]
        public async Task<IActionResult> GetSpoolThreads([FromRoute] string spoolName, [FromServices] ThreadService threadService) {            
            List<ThreadFull> threads = await threadService.GetThreadsBySpoolAsync(spoolName);
            return Ok(threads);
        }

        [HttpGet("{spoolId}")]
        public async Task<IActionResult> GetSpool([FromRoute] string spoolId, [FromServices] SpoolService spoolService)
        {
            Spool? spool = await spoolService.GetSpoolByNameAsync(spoolId);
            return Ok(spool);
        }

        [HttpPost("create")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostSpool([FromBody] PostSpoolRequest request, [FromServices] SpoolService spoolService) {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }

            Spool spool = new Spool
            {
                Name = request.Name,
                OwnerId = userDTO.Id,
                Interests = request.Interests,
                Moderators = request.Moderators
            };

            try
            {
                spool = await spoolService.InsertSpoolAsync(spool);
                return Ok(spool);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //can try the commented out one and remove userId from this route
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllSpools([FromRoute] string userId, [FromServices] SpoolService spoolService)
        {
            if (userId == null)
            {
                Spool[] spools = await spoolService.GetAllSpoolsAsync();
                return Ok(spools);
            }
            else
            {
                Spool[] spools = await spoolService.GetJoinedSpoolsAsync(userId);
                return Ok(spools);
            }
        }

        //different endpoint for commented out attempt in API
        [HttpGet("joined/{userId}")]
        public async Task<IActionResult> JoinedSpoolsEndpoint([FromRoute] string userId, [FromServices] SpoolService spoolService)
        {
            Spool[] spools = await spoolService.GetJoinedSpoolsAsync(userId);
            return Ok(spools);
        }
    }
}
