using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class Spool
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Interests { get; set; } = "";
        public string Mods { get; set; } = "";
        [Required]
        public User Owner { get; set; }
        public List<User> Moderators { get; set; } = new List<User>();
        public List<Thread> Threads { get; set; } = new List<Thread>();

        public Spool(User owner)
        {
            Owner = owner;
        }

    }
}