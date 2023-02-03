namespace ThreaditAPI.Models
{
    public class UserSettings
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool DarkMode { get; set; } = true;
        public string ProfilePicture { get; set; } = "";
        public string Interests { get; set; } = "";
        public string Spins { get; set; } = "";
        public string Spools { get; set; } = "";

    }
}