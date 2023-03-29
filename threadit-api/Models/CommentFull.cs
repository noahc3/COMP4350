using ThreaditAPI.Constants;

namespace ThreaditAPI.Models
{
    public class CommentFull : Comment
    {
        public string OwnerName { get; set; } = "";
        public string OwnerAvatar { get; set; } = UserConstants.DEFAULT_AVATAR_URL;
        public int ChildCommentCount { get; set; } = 0;


        public CommentFull()
        {

        }
    }
}
