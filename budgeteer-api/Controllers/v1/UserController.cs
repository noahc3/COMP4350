using BudgeteerAPI.Extensions;
using BudgeteerAPI.Middleware;
using BudgeteerAPI.Models.Requests;
using BudgeteerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgeteerAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase {
        [HttpGet("profile")]
        [AuthenticationRequired]
        public IActionResult GetProfile() {
            return Ok(Request.HttpContext.GetUserProfile());
        }
    }
}
