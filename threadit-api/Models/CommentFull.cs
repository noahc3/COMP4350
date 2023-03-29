using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ThreaditAPI.Models
{
    public class CommentFull : Comment
    {
        public string OwnerName { get; set; } = "";
        public int ChildCommentCount { get; set; } = 0;


        public CommentFull()
        {

        }
    }
}
