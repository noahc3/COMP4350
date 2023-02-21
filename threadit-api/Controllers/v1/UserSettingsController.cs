using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models;
using ThreaditAPI.Services;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/userSettings")]
    public class UserSettingsController : ControllerBase
    {
        [HttpGet("remove/{userId}/{spoolName}")]
        public async Task<IActionResult> RemoveSpoolUser([FromRoute] string userId, [FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
        {
            UserSettings userSettings = await userSettingsService.RemoveUserSettingsAsync(userId, spoolName);
            return Ok(userSettings);
        }
        
        [HttpGet("check/{userId}/{spoolName}")]
        public async Task<IActionResult> CheckSpoolUser([FromRoute] string userId, [FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
        {
            bool belongs = await userSettingsService.CheckSpoolUserAsync(userId, spoolName);
            return Ok(belongs);
        }
    }
}

