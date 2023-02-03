using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ThreaditAPI.Models
{
    public class Thread
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Topic { get; set; } = "";
        public string OwnerId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Rips { get; set; } = "";
        public string Stitches { get; set; } = "";
        [Required]
        public User Owner { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();


        public Thread(User owner)
        {
            Owner = owner;
        }
    }
}