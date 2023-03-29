using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using ThreaditAPI.Models;
using ThreaditAPI.Services;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/interest")]
    public class InterestController : ControllerBase
    {
        [HttpGet("all")]
        [AuthenticationRequired]
        public async Task<IActionResult> GetAllInterests([FromServices] InterestService interestService)
        {
            Interest[] interests = await interestService.GetAllInterestsAsync();
            return Ok(interests);
        }

        [HttpGet("add/{interestName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> AddInterest([FromRoute] string interestName, [FromServices] InterestService interestService)
        {
            UserDTO userDTO = Request.HttpContext.GetUser();

            Interest[] interest = await interestService.AddInterestAsync(interestName);
            return Ok(interest);
        }

        [HttpGet("remove/{interestName}")]
        [AuthenticationRequired]
        public async Task<IActionResult> RemoveInterest([FromRoute] string interestName, [FromServices] InterestService interestService)
        {
            UserDTO userDTO = Request.HttpContext.GetUser();

            Interest[] result = await interestService.RemoveInterestAsync(interestName);
            return Ok(result);
        }
    }
}
