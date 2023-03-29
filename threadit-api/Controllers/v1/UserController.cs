using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using ThreaditAPI.Models;
using System.Security.Cryptography;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        [HttpGet("profile")]
        [AuthenticationRequired]
        public IActionResult GetProfile()
        {
            UserDTO user = Request.HttpContext.GetUser();
        
            using (MD5 md5 = MD5.Create()) {
                byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Email.Trim().ToLower()));
                string gravatarHash = System.BitConverter.ToString(hash).Replace("-", "").ToLower();
                user.Avatar = $"https://www.gravatar.com/avatar/{gravatarHash}.jpg";
            }

            return Ok(user);
        }
    }
}
