namespace ThreaditAPI.Models
{
    public class Spool
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public required string OwnerId { get; set; }
        public List<string> Interests { get; set; } = new List<string>();
        public List<string> Moderators { get; set; } = new List<string>();
        public string Rules { get; set; } = "";


        public Spool()
        {

        }
    }
}
