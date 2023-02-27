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

        [HttpGet("{spoolName}")]
        public async Task<IActionResult> GetSpool([FromRoute] string spoolName, [FromServices] SpoolService spoolService)
        {
            Spool? spool = await spoolService.GetSpoolByNameAsync(spoolName);
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpools([FromServices] SpoolService spoolService)
        {
            Spool[] spools = await spoolService.GetAllSpoolsAsync();
            return Ok(spools);
        }

        [HttpGet("joined/{userId}")]
        public async Task<IActionResult> JoinedSpools([FromRoute] string userId, [FromServices] SpoolService spoolService)
        {
            List<Spool> spools = await spoolService.GetJoinedSpoolsAsync(userId);
            return Ok(spools);
        }

        [HttpGet("mods/{spoolId}")]
        public async Task<IActionResult> AllModsForSpool([FromRoute] string spoolId, [FromServices] SpoolService spoolService)
        {
            UserDTO[]? users = await spoolService.GetAllModsForSpoolAsync(spoolId);
            return Ok(users);
        }

        [HttpGet("mods/add/{spoolId}/{userId}")]
        public async Task<IActionResult> AddModerator([FromRoute] string spoolId, [FromRoute] string userId, [FromServices] SpoolService spoolService)
        {
            SpoolController? spool;
            try { 
                spool = await spoolService.AddModeratorAsync(spoolId, userId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(spool);
        }

        [HttpGet("mods/remove/{spoolId}/{userId}")]
        public async Task<IActionResult> RemoveModerator([FromRoute] string spoolId, [FromRoute] string userId, [FromServices] SpoolService spoolService)
        {
            Spool? spool = await spoolService.RemoveModeratorAsync(spoolId, userId);
            return Ok(spool);
        }

        [HttpGet("delete/{spoolId}")]
        public async Task<IActionResult> ChangeOwner([FromRoute] string spoolId, [FromServices] SpoolService spoolService)
        {
            try
            {
                await spoolService.DeleteSpoolAsync(spoolId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save/{spoolId}")]
        public async Task<IActionResult> SaveRules([FromRoute] string spoolId, [FromBody] SaveRulesRequest rules, [FromServices] SpoolService spoolService)
        {
            try
            {
                await spoolService.SaveRulesAsync(spoolId, rules.Rules);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
