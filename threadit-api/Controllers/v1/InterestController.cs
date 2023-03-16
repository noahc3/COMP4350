using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models;
using ThreaditAPI.Services;
using Microsoft.Net.Http.Headers;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/interests")]
    public class InterestController : ControllerBase {
        [HttpGet("all")]
        [AuthenticationRequired]
        public async Task<IActionResult> GetAllInterests([FromServices] InterestService interestService) {
            Interest[] interests = await interestService.GetAllInterestsAsync();
            return Ok(interests);
        }
    }
}
