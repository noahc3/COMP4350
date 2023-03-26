using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Constants;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class ThreadService
    {
        private readonly ThreadRepository threadRepository;
        private readonly CommentRepository commentRepository;
        private readonly SpoolRepository spoolRepository;
        private readonly UserRepository userRepository;
        public ThreadService(PostgresDbContext context)
        {
            this.threadRepository = new ThreadRepository(context);
            this.commentRepository = new CommentRepository(context);
            this.spoolRepository = new SpoolRepository(context);
            this.userRepository = new UserRepository(context);
        }

        private async Task<ThreadFull> ConvertToThreadFull(Models.Thread thread) {
            return (await ConvertToThreadFull(new Models.Thread[] { thread }))[0];
        }

        private async Task<ThreadFull[]> ConvertToThreadFull(Models.Thread[] threads) {
            Dictionary<string, Spool> spools = new Dictionary<string, Spool>();
            Dictionary<string, UserDTO> users = new Dictionary<string, UserDTO>();
            List<Models.ThreadFull> fullThreads = new List<Models.ThreadFull>();
            for (int i = 0; i < threads.Count(); i++)
            {
                UserDTO user;
                Spool spool;

                if (users.ContainsKey(threads[i].OwnerId)) {
                    user = users[threads[i].OwnerId];
                } else {
                    user = (await userRepository.GetUserAsync(threads[i].OwnerId))!;
                    users[threads[i].OwnerId] = user;
                }

                if (spools.ContainsKey(threads[i].SpoolId)) {
                    spool = spools[threads[i].SpoolId];
                } else {
                    spool = (await spoolRepository.GetSpoolAsync(threads[i].SpoolId))!;
                    spools[threads[i].SpoolId] = spool;
                }

                Models.ThreadFull fullThread = new Models.ThreadFull()
                {
                    Id = threads[i].Id,
                    Content = threads[i].Content,
                    Topic = threads[i].Topic,
                    Title = threads[i].Title,
                    SpoolId = threads[i].SpoolId,
                    OwnerId = threads[i].OwnerId,
                    DateCreated = threads[i].DateCreated,
                    Stitches = threads[i].Stitches,
                    Rips = threads[i].Rips,
                    AuthorName = user.Username,
                    SpoolName = spool.Name,
                    CommentCount = await commentRepository.TotalThreadCommentCount(threads[i].Id),
                    TopLevelCommentCount = await commentRepository.TopLevelThreadCommentCount(threads[i].Id),
                    ThreadType = threads[i].ThreadType
                };
                fullThreads.Add(fullThread);
            }

            return fullThreads.ToArray();
        }

        private async Task<bool> IsDeleteAuthorized(string threadId, string userId)
        {
            Models.Thread? thread = await this.threadRepository.GetThreadAsync(threadId);

            if (thread == null)
            {
                return false;
            }

            if (userId == thread.OwnerId)
            {
                return true;
            }

            Spool spool = (await this.spoolRepository.GetSpoolAsync(thread.SpoolId))!;

            return spool.OwnerId == userId || spool.Moderators.Contains(userId);
        }

        public async Task<Models.Thread> GetThreadAsync(string threadId)
        {
            Models.Thread? returnedThread = await this.threadRepository.GetThreadAsync(threadId);
            if (returnedThread != null)
            {
                return returnedThread;
            }
            else
            {
                throw new Exception("Thread does not exist.");
            }
        }

        public async Task<Models.ThreadFull?> GetThreadFullAsync(string threadId)
        {
            Models.Thread? returnedThread = await this.threadRepository.GetThreadAsync(threadId);

            if (returnedThread == null)
            {
                throw new Exception("Thread does not exist.");
            }

            return await ConvertToThreadFull(returnedThread);
        }

        public async Task<Models.ThreadFull[]> GetThreadsBySpoolAsync(string spoolName, string? sort = null, string? searchQuery = null, int? skip = null)
        {
            Models.Spool? spool = await spoolRepository.GetSpoolByNameAsync(spoolName);

            if (spool == null)
            {
                throw new Exception("Spool does not exist.");
            }
            
            Models.Thread[] threads = await this.threadRepository.GetThreadsAsync(sort, searchQuery, skip, spool.Id);
            return await ConvertToThreadFull(threads);
        }

        public async Task<Models.ThreadFull[]> GetThreadsAsync(string? sort = null, string? searchQuery = null, int? skip = null, string? spoolId = null)
        {
            Models.Thread[] threads = await this.threadRepository.GetThreadsAsync(sort, searchQuery, skip, spoolId);
            return await ConvertToThreadFull(threads);
        }

        public async Task<Models.Thread> InsertThreadAsync(Models.Thread thread)
        {
            if (string.IsNullOrWhiteSpace(thread.Title))
            {
                throw new Exception("Please enter a valid thread title.");
            }
            if (thread.Title.Length > 256)
            {
                throw new Exception("Thread title maximum is 256 characters. Please shorten title.");
            }
            if (thread.Content.Length > 2048)
            {
                throw new Exception("Thread content maximum is 2048 Characters. Current content length is: " + thread.Content.Length + ". Please Shorten content.");
            }
            if (!ThreadTypes.types.Contains(thread.ThreadType)) {
                throw new Exception($"Thread type must be one of {ThreadTypes.typesString}.");
            }



            Models.Thread? dbThread = await this.threadRepository.GetThreadAsync(thread.Id);
            if (dbThread != null)
            {
                throw new Exception("Thread id already in use. Please pick a new id.");
            }

            await this.threadRepository.InsertThreadAsync(thread);
            return thread;
        }

        public async Task<Models.Thread> UpdateThreadAsync(Models.Thread thread)
        {
            if (thread.Content.Length > 2048)
            {
                throw new Exception("Thread content maximum is 2048 Characters. Current content length is: " + thread.Content.Length + ". Please Shorten content.");
            }
            await this.threadRepository.UpdateThreadAsync(thread);
            return thread;
        }

        public async Task DeleteThreadAsync(string threadId, string userId)
        {
            if (!await IsDeleteAuthorized(threadId, userId))
            {
                throw new Exception("User does not have permission to delete comment.");
            }

            await this.commentRepository.HardDeleteAllThreadCommentsAsync(threadId);
            await this.threadRepository.DeleteThreadAsync(threadId);
        }
    }
}
