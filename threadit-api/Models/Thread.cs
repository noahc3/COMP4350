using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ThreaditAPI.Models
{
    public class Thread
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Topic { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<string> Rips { get; set; } = new List<string>();
        public List<string> Stitches { get; set; } = new List<string>();
        public required string OwnerId { get; set; }
        public required string SpoolId { get; set; }


        public Thread()
        {

        }
    }
}