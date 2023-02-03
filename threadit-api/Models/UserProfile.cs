namespace ThreaditAPI.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";

        public UserProfile()
        {

        }

        public UserProfile(User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.Username = user.Username;
        }
    }
}