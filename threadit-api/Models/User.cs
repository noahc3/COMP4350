using System;

namespace ThreaditAPI.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
