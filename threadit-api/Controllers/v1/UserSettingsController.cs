using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using ThreaditAPI.Models;
using ThreaditAPI.Services;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/userSettings")]
    public class UserSettingsController : ControllerBase
    {

        [HttpGet("remove/{spoolName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> RemoveSpoolUser([FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }
            UserSettings userSettings = await userSettingsService.RemoveUserSettingsAsync(userDTO.Id, spoolName);
            return Ok(userSettings);
        }

        [HttpGet("join/{spoolName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> JoinSpoolUser([FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }
            UserSettings userSettings = await userSettingsService.JoinUserSettingsAsync(userDTO.Id, spoolName);
            return Ok(userSettings);
        }

        [HttpGet("check/{spoolName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> CheckSpoolUser([FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }
            bool belongs = await userSettingsService.CheckSpoolUserAsync(userDTO.Id, spoolName);
            return Ok(belongs);
        }

        [HttpGet("check/newUser")]
        [AuthenticationRequired]
        public async Task<IActionResult> NewUser([FromServices] UserSettingsService userSettingsService)
        {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null)
            {
                return Unauthorized();
            }
            bool newUser = await userSettingsService.CheckNewUserAsync(userDTO.Id);
            return Ok(newUser);
        }

    }
}

