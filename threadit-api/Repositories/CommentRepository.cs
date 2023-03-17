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
        private const int TOP_LEVEL_BASE_REPLY_COUNT = 2;
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

            List<Comment> replies = new List<Comment>();

            foreach (Comment comment in comments) {
                replies.AddRange(await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == comment.Id)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(TOP_LEVEL_BASE_REPLY_COUNT)
                                                  .ToListAsync());
            }
            
            comments.AddRange(replies);

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

            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == sibling.ParentCommentId && c.DateCreated >= sibling.DateCreated)
                                                  .OrderBy((c) => c.DateCreated)
                                                  .Take(sibling.ParentCommentId == null ? TOP_LEVEL_EXPAND_COUNT + 1 : SUB_LEVEL_EXPAND_COUNT + 1)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .ToListAsync();

            return comments;
        }

        public async Task<List<Comment>> OlderComments(string threadId, string siblingCommentId) {
            Comment? sibling = await GetCommentAsync(siblingCommentId);

            if (sibling == null) {
                throw new Exception("Sibling comment does not exist.");
            }

            List<Comment> comments = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == sibling.ParentCommentId && c.DateCreated <= sibling.DateCreated)
                                                  .OrderByDescending((c) => c.DateCreated)
                                                  .Take(sibling.ParentCommentId == null ? TOP_LEVEL_EXPAND_COUNT + 1 : SUB_LEVEL_EXPAND_COUNT + 1)
                                                  .ToListAsync();

            if (sibling.ParentCommentId == null) {
                List<Comment> replies = new List<Comment>();

                foreach (Comment comment in comments) {
                    replies.AddRange(await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == comment.Id)
                                                    .OrderByDescending((c) => c.DateCreated)
                                                    .Take(TOP_LEVEL_BASE_REPLY_COUNT)
                                                    .ToListAsync());
                }
                
                comments.AddRange(replies);
            }

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

        public async Task<Comment?> DeleteCommentAsync(string commentId)
        {
            Comment? comment = await GetCommentAsync(commentId);
            if (comment != null)
            {
                comment.OwnerId = UserConstants.USER_DELETED_TEXT;
                comment.Content = UserConstants.COMMENT_DELETED_TEXT;
                comment.IsDeleted = true;
                await db.SaveChangesAsync();
            }

            return comment;
        }

        public async Task HardDeleteAllSpoolCommentsAsync(string spoolId) {
            await db.Comments.Join(db.Threads, c => c.ThreadId, t => t.Id, (c, t) => new { Comment = c, Thread = t })
                             .Where(c => c.Thread.SpoolId == spoolId)
                             .ForEachAsync(c => db.Comments.Remove(c.Comment));
            await db.SaveChangesAsync();
        }

        public async Task HardDeleteAllThreadCommentsAsync(string threadId) {
            await db.Comments.Where(c => c.ThreadId == threadId)
                             .ForEachAsync(c => db.Comments.Remove(c));
            await db.SaveChangesAsync();
        }

        public async Task<int> ImmediateChildCommentCountAsync(string commentId) {
            int count = await db.Comments.Where(c => c.ParentCommentId == commentId).CountAsync();
            return count;
        }

        public async Task<int> TotalThreadCommentCount(string threadId) {
            int count = await db.Comments.Where(c => c.ThreadId == threadId).CountAsync();
            return count;
        }

        public async Task<int> TopLevelThreadCommentCount(string threadId) {
            int count = await db.Comments.Where(c => c.ThreadId == threadId && c.ParentCommentId == null).CountAsync();
            return count;
        }
    }
}
