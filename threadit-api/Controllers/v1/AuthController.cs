using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models.Requests;
using ThreaditAPI.Services;
using ThreaditAPI.Models;
using ThreaditAPI.Middleware;
using Microsoft.Net.Http.Headers;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/auth")]
    public class AuthController : ControllerBase {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateAccountRequest request, [FromServices] UserService userService) {
            try {
                User user = await userService.CreateUserAsync(request.Username, request.Email, request.Password);
                return Ok();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }   
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromServices] UserService userService, [FromServices] UserSessionService sessionService) {
            User? user;
            try {
                user = await userService.GetUserAsync(request.Username, request.Password);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

            if (user == null) {
                return Unauthorized();
            }

            UserSession session = await sessionService.CreateUserSessionAsync(user);

            return Ok(session.Id);
        } 

        [HttpPost("checksession")]
        public async Task<IActionResult> CheckSession([FromBody] CheckSessionRequest request, [FromServices] UserSessionService sessionService) {
            UserSession? session = await sessionService.GetUserSessionAsync(request.Token);
            if (session != null && session.DateExpires > DateTime.Now) {
                return Ok();
            } else {
                return Unauthorized();
            }
        }

        [HttpGet("logout")]
        [AuthenticationRequired]
        public async Task<IActionResult> Logout([FromServices] UserSessionService userSessionService) {
            string sessionToken = Request.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            await userSessionService.DeleteUserSessionAsync(sessionToken);
            
            return Ok();
        }
    }
}
