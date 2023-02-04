using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class Spool
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Interests { get; set; } = "";
        [Required]
        public User Owner { get; set; }
        public List<ModeratorProfile> Moderators { get; set; } = new List<ModeratorProfile>();
        public List<Thread> Threads { get; set; } = new List<Thread>();

        public Spool()
        {

        }
        public Spool(User owner)
        {
            Owner = owner;
        }
    }
}