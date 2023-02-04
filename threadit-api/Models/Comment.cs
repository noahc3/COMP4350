using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; } = "";
        public bool Edited { get; set; } = false;
        [Required]
        public User Owner { get; set; }

        public Comment()
        {

        }
        public Comment(User owner)
        {
            Owner = owner;
        }

    }
}