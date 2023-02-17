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
            List<Models.ThreadFull> threads = await threadService.GetThreadsBySpoolAsync(spoolName);
            return Ok(threads);
        }

        [HttpGet("{spoolId}")]
        public async Task<IActionResult> GetSpool([FromRoute] string spoolId, [FromServices] SpoolService spoolService)
        {
            Models.Spool? spool = await spoolService.GetSpoolByNameAsync(spoolId);
            return Ok(spool);
        }

        [HttpPost("create")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostSpool([FromBody] PostSpoolRequest request, [FromServices] SpoolService spoolService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }

            Models.Spool spool = new Models.Spool
            {
                Id = request.Id,
                Name = request.Name,
                OwnerId = userDTO.Id,
                Interests = request.Interests,
                Moderators = request.Moderators
            };

            try
            {
                spool = await spoolService.InsertSpoolAsync(spool);
                return Ok(spool);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}