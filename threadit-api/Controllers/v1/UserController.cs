using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models;
using ThreaditAPI.Services;
using Microsoft.Net.Http.Headers;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase {
        [HttpGet("profile")]
        [AuthenticationRequired]
        public IActionResult GetProfile() {
            return Ok(Request.HttpContext.GetUser());
        }

        [HttpGet("logout")]
        [AuthenticationRequired]
        public async Task<IActionResult> Logout([FromServices] UserSessionService userSessionService) {
            try {
                string sessionToken = Request.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                await userSessionService.DeleteUserSessionAsync(sessionToken);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
            
            return Ok();
        }
    }
}
