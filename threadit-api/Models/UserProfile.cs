namespace ThreaditAPI.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<Spool> CreatedSpools { get; set; } = new List<Spool>();

        public UserProfile()
        {

        }

        public UserProfile(User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.Username = user.Username;
            this.DateCreated = user.DateCreated;
            this.CreatedSpools = user.CreatedSpools;
        }
    }
}