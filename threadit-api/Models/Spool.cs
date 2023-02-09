using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class Spool
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Interests { get; set; } = "";
        public required string OwnerId { get; set; }
        //each string is a user Id
        public List<string> Moderators { get; set; } = new List<string>();

        public Spool()
        {

        }
        public Spool(string ownerId)
        {
            this.OwnerId = ownerId;
        }
    }
}