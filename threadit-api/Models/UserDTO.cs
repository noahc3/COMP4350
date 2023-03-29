using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThreaditAPI.Constants;

namespace ThreaditAPI.Models
{
    public class UserDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Email { get; set; }
        public required string Username { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string Avatar { get; set; } = UserConstants.DEFAULT_AVATAR_URL;
        public UserDTO()
        {

        }
    }
}
