using System.ComponentModel.DataAnnotations;

namespace ThreaditAPI.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; } = "";
        public bool Edited { get; set; } = false;
        public required string OwnerId { get; set; }
        public required string ThreadId { get; set; }
        public required string? ParentCommentId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public Comment()
        {

        }
        public Comment(string ownerId, string threadId)
        {
            this.OwnerId = ownerId;
            this.ThreadId = threadId;
        }
        public Comment(string ownerId, string threadId, string parentCommentId)
        {
            this.OwnerId = ownerId;
            this.ThreadId = threadId;
            this.ParentCommentId = parentCommentId;
        }
    }
}