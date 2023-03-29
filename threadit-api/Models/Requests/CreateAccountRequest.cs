using System.Text.Json.Serialization;
namespace ThreaditAPI.Models.Requests
{
	public class CreateAccountRequest
	{
		public string Username { get; set; } = "";
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
		public string ConfirmPassword { get; set; } = "";
		public Boolean NewUser { get; set; } = true;
	}
}
