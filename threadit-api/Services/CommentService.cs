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
            return await this.commentRepository.GetCommentAsync(commentId);
        }

        public async Task<Comment?> GetCommentAsync(Comment comment)
        {
            return await this.commentRepository.GetCommentAsync(comment);
        }

        public async Task<Comment> InsertCommentAsync(Comment comment)
        {
            await this.commentRepository.InsertCommentAsync(comment);
            return comment;
        }

    }
}
