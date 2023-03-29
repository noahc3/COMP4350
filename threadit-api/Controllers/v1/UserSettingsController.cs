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
			UserDTO userDTO = Request.HttpContext.GetUser();

			UserSettings userSettings = await userSettingsService.RemoveUserSettingsAsync(userDTO.Id, spoolName);
			return Ok(userSettings);
		}

		[HttpGet("join/{spoolName}")]
		[AuthenticationRequired]
		public async Task<IActionResult> JoinSpoolUser([FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();

			UserSettings userSettings = await userSettingsService.JoinUserSettingsAsync(userDTO.Id, spoolName);
			return Ok(userSettings);
		}

		[HttpGet("check/{spoolName}")]
		[AuthenticationRequired]
		public async Task<IActionResult> CheckSpoolUser([FromRoute] string spoolName, [FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();

			bool belongs = await userSettingsService.CheckSpoolUserAsync(userDTO.Id, spoolName);
			return Ok(belongs);
		}

		[HttpGet("interests")]
		[AuthenticationRequired]
		public async Task<IActionResult> GetInterests([FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();

			UserSettings settings = (await userSettingsService.GetUserSettingsAsync(userDTO.Id))!;
			return Ok(settings.Interests.ToArray());
		}

		[HttpGet("addInterest/{interestName}")]
		[AuthenticationRequired]
		public async Task<IActionResult> AddInterest([FromRoute] string interestName, [FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();
			string[] interests = await userSettingsService.AddUserInterestAsync(userDTO.Id, interestName);
			return Ok(interests);
		}

		[HttpGet("removeInterest/{interestName}")]
		[AuthenticationRequired]
		public async Task<IActionResult> RemoveInterest([FromRoute] string interestName, [FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();
			string[] interests = await userSettingsService.RemoveUserInterestAsync(userDTO.Id, interestName);
			return Ok(interests);
		}

		[HttpGet("belong/{interestName}")]
		[AuthenticationRequired]

		public async Task<IActionResult> BelongInterest([FromRoute] string interestName, [FromServices] UserSettingsService userSettingsService)
		{
			UserDTO userDTO = Request.HttpContext.GetUser();
			bool belong = await userSettingsService.BelongInterestAsync(userDTO.Id, interestName);
			return Ok(belong);
		}
	}
}

