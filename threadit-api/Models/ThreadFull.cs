using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ThreaditAPI.Models
{
    public class ThreadFull : Thread
    {
        public string AuthorName { get; set; } = "";
        public string SpoolName { get; set; } = "";
        public int CommentCount { get; set; } = 0;
        public int TopLevelCommentCount { get; set; } = 0;


        public ThreadFull()
        {

        }
    }
}