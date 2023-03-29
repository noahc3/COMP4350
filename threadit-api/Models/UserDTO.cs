using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
	public class UserDTO
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public required string Email { get; set; }
		public required string Username { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
		public UserDTO()
		{

		}
	}
}
