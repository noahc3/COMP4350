namespace ThreaditAPI.Models
{
    public class User : UserProfile
    {
        public string PasswordHash { get; set; } = "";
    }
}