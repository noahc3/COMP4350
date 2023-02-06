using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class UserDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public UserDTO()
        {

        }

        public UserDTO(string email, string username)
        {
            this.Email = email;
            this.Username = username;
        }
    }
}