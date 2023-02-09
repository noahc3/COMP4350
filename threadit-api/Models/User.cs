namespace ThreaditAPI.Models
{
    public class User : UserDTO
    {
        public string PasswordHash { get; set; } = "";

        public User()
        {

        }

        public User(string email, string username) : base(email, username)
        {
        }
    }
}