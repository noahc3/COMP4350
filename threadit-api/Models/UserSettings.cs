namespace ThreaditAPI.Models
{
    public class UserSettings
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool DarkMode { get; set; } = true;
        public string ProfilePicture { get; set; } = "";
        public List<string> Interests { get; set; } = new List<string>();
        //each string is a thread Id
        public List<string> Spins { get; set; } = new List<string>();
        //each string is a spool Id
        public  List<string> SpoolsJoined { get; set; } = new List<string>();
    }
}