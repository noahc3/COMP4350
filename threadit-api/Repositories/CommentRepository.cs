using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Constants;

namespace ThreaditAPI.Repositories
{
    public class CommentRepository : AbstractRepository
    {
        private const int TOP_LEVEL_EXPAND_COUNT = 10;
        private const int SUB_LEVEL_EXPAND_COUNT = 3;
        public CommentRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Comment?> GetCommentAsync(string commentId)
        {
            Comment? dbComment = await db.Comments.FirstOrDefaultAsync(u => u.Id == commentId);
            return dbComment == default ? null : dbComment;
        }

        public async Task<List<Comment>> GetBaseComments(string threadId) {
            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == null)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(TOP_LEVEL_EXPAND_COUNT)
                                                  .ToListAsync();
            return comments;
        }

        public async Task<List<Comment>> ExpandComments(string threadId, string parentCommentId) {
            Comment? parentComment = await GetCommentAsync(parentCommentId);
            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == parentCommentId)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(SUB_LEVEL_EXPAND_COUNT)
                                                  .ToListAsync();

            if (parentComment != null) {
                comments.Add(parentComment);
            }

            return comments;
        }

        public async Task<List<Comment>> NewerComments(string threadId, string siblingCommentId) {
            Comment? sibling = await GetCommentAsync(siblingCommentId);

            if (sibling == null) {
                throw new Exception("Sibling comment does not exist.");
            }

            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == sibling.ParentCommentId && c.DateCreated > sibling.DateCreated)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(SUB_LEVEL_EXPAND_COUNT)
                                                  .ToListAsync();

            comments.Add(sibling);

            return comments;
        }

        public async Task<List<Comment>> OlderComments(string threadId, string siblingCommentId) {
            Comment? sibling = await GetCommentAsync(siblingCommentId);

            if (sibling == null) {
                throw new Exception("Sibling comment does not exist.");
            }

            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == sibling.ParentCommentId && c.DateCreated < sibling.DateCreated)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(SUB_LEVEL_EXPAND_COUNT)
                                                  .ToListAsync();

            comments.Add(sibling);

            return comments;
        }

        public async Task<Comment> InsertCommentAsync(Comment comment)
        {
            await db.Comments.AddAsync(comment);
            await db.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            Comment? dbComment = await GetCommentAsync(comment.Id);
            if (dbComment != null)
            {
                dbComment.Content = comment.Content;
                await db.SaveChangesAsync();
                return dbComment;
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<Comment> DeleteCommentAsync(string commentId)
        {
            Comment? comment = await GetCommentAsync(commentId);
            if (comment != null)
            {
                comment.OwnerId = UserConstants.USER_DELETED_TEXT;
                comment.Content = UserConstants.COMMENT_DELETED_TEXT;
                comment.IsDeleted = true;
                await db.SaveChangesAsync();
                return comment;
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<int> ImmediateChildCommentCountAsync(string commentId) {
            int count = await db.Comments.Where(c => c.ParentCommentId == commentId).CountAsync();
            return count;
        }
    }
}
