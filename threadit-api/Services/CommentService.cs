using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class CommentService
    {
        private readonly CommentRepository commentRepository;
        public CommentService(PostgresDbContext context)
        {
            this.commentRepository = new CommentRepository(context);
        }

        public async Task<Comment?> GetCommentAsync(string commentId)
        {
            Comment? returnedComment = await this.commentRepository.GetCommentAsync(commentId);
            if (returnedComment != null)
            {
                return returnedComment;
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<Comment?> GetCommentAsync(Comment comment)
        {
            Comment? returnedComment = await this.commentRepository.GetCommentAsync(comment);
            if (returnedComment != null)
            {
                return returnedComment;
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<Comment> InsertCommentAsync(Comment comment)
        {
            await this.commentRepository.InsertCommentAsync(comment);
            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            Comment? returnedComment = await this.commentRepository.UpdateCommentAsync(comment);
            if (returnedComment != null)
            {
                return comment;
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }
    }
}
