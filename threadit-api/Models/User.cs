namespace ThreaditAPI.Models
{
	public class User : UserDTO
	{
		public string PasswordHash { get; set; } = "";

		public User()
		{

		}
	}
}
