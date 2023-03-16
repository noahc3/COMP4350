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

        [HttpGet("add/{interestName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> AddInterest([FromRoute] string interestName, [FromServices] InterestService interestService) {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null) {
                return Unauthorized();
            }

            Interest[] interest = await interestService.AddInterestAsync(interestName);
            return Ok(interest);
        }

        [HttpGet("remove/{interestName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> RemoveInterest([FromRoute] string interestName, [FromServices] InterestService interestService) {
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null) {
                return Unauthorized();
            }

            Interest[] result = await interestService.RemoveInterestAsync(interestName);
            return Ok(result);
        }
    }
}
