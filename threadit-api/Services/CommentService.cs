using ThreaditAPI.Constants;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class CommentService
    {
        private readonly CommentRepository commentRepository;
        private readonly UserRepository userRepository;
        private readonly ThreadRepository threadRepository;
        private readonly SpoolRepository spoolRepository;
        public CommentService(PostgresDbContext context)
        {
            this.commentRepository = new CommentRepository(context);
            this.userRepository = new UserRepository(context);
            this.threadRepository = new ThreadRepository(context);
            this.spoolRepository = new SpoolRepository(context);
        }

        private async Task<bool> IsDeleteAuthorized(string userId, Comment comment) {
            if (userId == comment.OwnerId) {
                return true;
            }
            
            Models.Thread? thread = await this.threadRepository.GetThreadAsync(comment.ThreadId);
            if (thread == null) {
                return false;
            }

            Spool? spool = await this.spoolRepository.GetSpoolAsync(thread.SpoolId);

            if (spool == null) {
                return false;
            }

            return spool.OwnerId == userId || spool.Moderators.Contains(userId);
        }

        private async Task<CommentFull> ConvertToCommentFull(Comment comment) {
            return (await ConvertToCommentFull(new Comment[] { comment }))[0];
        }

        private async Task<CommentFull[]> ConvertToCommentFull(IEnumerable<Comment> comments) {
            Dictionary<string, string> usernames = new Dictionary<string, string>();
            List<CommentFull> results = new List<CommentFull>();

            foreach (Comment c in comments) {
                int childCommentCount = await this.commentRepository.ImmediateChildCommentCountAsync(c.Id);
                string username;
                
                if (c.IsDeleted) {
                    username = c.OwnerId;
                } else if (usernames.ContainsKey(c.OwnerId)) {
                    username = usernames[c.OwnerId];
                } else {
                    UserDTO? user = await this.userRepository.GetUserAsync(c.OwnerId);
                    if (user == null) {
                        username = UserConstants.USER_DELETED_TEXT;
                    } else {
                        username = user.Username;
                    }
                    usernames.Add(c.OwnerId, username);
                }

                CommentFull full = new CommentFull() {
                    Id = c.Id,
                    ThreadId = c.ThreadId,
                    ParentCommentId = c.ParentCommentId,
                    OwnerId = c.OwnerId,
                    OwnerName = username,
                    Content = c.Content,
                    DateCreated = c.DateCreated,
                    IsDeleted = c.IsDeleted,
                    ChildCommentCount = childCommentCount
                };

                results.Add(full);
            }

            return results.ToArray();
        }

        public async Task<CommentFull> GetCommentAsync(string commentId)
        {
            Comment? returnedComment = await this.commentRepository.GetCommentAsync(commentId);
            if (returnedComment != null)
            {
                return await ConvertToCommentFull(returnedComment);
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<CommentFull[]> GetBaseComments(string threadId) {
            List<Comment> comments = await this.commentRepository.GetBaseComments(threadId);

            return await ConvertToCommentFull(comments);
        }

        public async Task<CommentFull[]> ExpandComments(string threadId, string parentCommentId) {
            List<Comment> comments = await this.commentRepository.ExpandComments(threadId, parentCommentId);

            return await ConvertToCommentFull(comments);
        }

        public async Task<CommentFull[]> NewerComments(string threadId, string siblingCommentId) {
            List<Comment> comments = await this.commentRepository.NewerComments(threadId, siblingCommentId);

            return await ConvertToCommentFull(comments);
        }

        public async Task<CommentFull[]> OlderComments(string threadId, string siblingCommentId) {
            List<Comment> comments = await this.commentRepository.OlderComments(threadId, siblingCommentId);

            return await ConvertToCommentFull(comments);
        }

        public async Task<Comment> InsertCommentAsync(Comment comment)
        {
            await this.commentRepository.InsertCommentAsync(comment);
            return await ConvertToCommentFull(comment);
        }

        public async Task<Comment> UpdateCommentAsync(string userId, Comment comment)
        {
            Comment? returnedComment = await this.commentRepository.UpdateCommentAsync(comment);
            if (returnedComment != null)
            {
                if (returnedComment.OwnerId != userId) {
                    throw new Exception("User does not own comment.");
                }

                return await ConvertToCommentFull(returnedComment);
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }

        public async Task<Comment> DeleteCommentAsync(string userId, string commentId)
        {
            Comment? returnedComment = await this.commentRepository.GetCommentAsync(commentId);
            if (returnedComment != null)
            {
                if (!await IsDeleteAuthorized(userId, returnedComment)) {
                    throw new Exception("User does not have permission to delete comment.");
                }

                returnedComment = await this.commentRepository.DeleteCommentAsync(returnedComment.Id);
                return await ConvertToCommentFull(returnedComment);
            }
            else
            {
                throw new Exception("Comment does not exist.");
            }
        }
    }
}
