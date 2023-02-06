using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class CommentRepository : AbstractRepository
    {
        public CommentRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Comment?> GetCommentAsync(Comment comment)
        {
            return await GetCommentAsync(comment.Id);
        }

        public async Task<Comment?> GetCommentAsync(string commentId)
        {
            Comment? dbComment = await db.Comments.FirstOrDefaultAsync(u => u.Id == commentId);
            return dbComment == default ? null : dbComment;
        }

        public async Task InsertCommentAsync(Comment comment)
        {
            await db.Comments.AddAsync(comment);
            await db.SaveChangesAsync();
        }
    }
}
