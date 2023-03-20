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

        private async Task<bool> IsDeleteAuthorized(string threadId, string userId) {
            Models.Thread? thread = await this.threadRepository.GetThreadAsync(threadId);

            if (thread == null) {
                return false;
            }

            if (userId == thread.OwnerId) {
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
            if (returnedThread != null)
            {
                Models.Spool spool = (await spoolRepository.GetSpoolAsync(returnedThread.SpoolId))!;
                UserDTO user = (await userRepository.GetUserAsync(returnedThread.OwnerId))!;

                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = returnedThread.Id,
                    Content = returnedThread.Content,
                    Topic = returnedThread.Topic,
                    Title = returnedThread.Title,
                    SpoolId = returnedThread.SpoolId,
                    OwnerId = returnedThread.OwnerId,
                    DateCreated = returnedThread.DateCreated,
                    Stitches = returnedThread.Stitches,
                    Rips = returnedThread.Rips,
                    AuthorName = user.Username,
                    SpoolName = spool.Name,
                    CommentCount = await commentRepository.TotalThreadCommentCount(returnedThread.Id),
                    TopLevelCommentCount = await commentRepository.TopLevelThreadCommentCount(returnedThread.Id)
                };
                return fullThread;
            }          
            else
            {
                throw new Exception("Thread does not exist.");
            }
        }

        public async Task<List<Models.ThreadFull>> GetThreadsBySpoolAsync(string spoolName)
        {
            Models.Spool? spool = await spoolRepository.GetSpoolByNameAsync(spoolName);

            if (spool == null)
            {
                throw new Exception("Spool does not exist.");
            }
            
            Models.Thread[] threads = await this.threadRepository.GetThreadsBySpoolAsync(spool.Id);
            List<Models.ThreadFull> fullThreads = new List<Models.ThreadFull>();
            for (int i = 0; i < threads.Length; i++)
            {
                UserDTO user = (await userRepository.GetUserAsync(threads[i].OwnerId))!;

                Models.ThreadFull fullThread = new Models.ThreadFull() {
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
                    TopLevelCommentCount = await commentRepository.TopLevelThreadCommentCount(threads[i].Id)
                };
                fullThreads.Add(fullThread);
            }

            return fullThreads;
        }

        public async Task<Models.ThreadFull[]> GetAllThreadsAsync()
        {
            Models.Thread[] threads = await this.threadRepository.GetAllThreadsAsync();
            Models.ThreadFull[] fullThreads = new Models.ThreadFull[threads.Length];
            for (int i = 0; i < threads.Length; i++)
            {
                UserDTO user = (await new UserService(new PostgresDbContext()).GetUserAsync(threads[i].OwnerId))!;
                Spool spool = (await new SpoolService(new PostgresDbContext()).GetSpoolAsync(threads[i].SpoolId))!;

                Models.ThreadFull fullThread = new Models.ThreadFull() {
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
                    TopLevelCommentCount = await commentRepository.TopLevelThreadCommentCount(threads[i].Id)
                };
                fullThreads[i] = fullThread;
            }

            return fullThreads;
        }

        public async Task<Models.Thread> InsertThreadAsync(Models.Thread thread)
        {
            if (thread.Title.IsNullOrEmpty())
            {
                throw new Exception("Please enter a valid thread title.");
            }
            if (spool.Title.Length > 256)
            {
                throw new Exception("Thread title maximum is 256 characters. Please shorten title.");
            }
            if(thread.Content.Length > 2048)
            {
                throw new Exception("Thread content maximum is 2048 Characters. Current content length is: " + thread.Content.Length() + ". Please Shorten content.");
            }
            await this.threadRepository.InsertThreadAsync(thread);
            return thread;
        }

        public async Task<Models.Thread> UpdateThreadAsync(Models.Thread thread)
        {
            await this.threadRepository.UpdateThreadAsync(thread);
            return thread;
        }

        public async Task DeleteThreadAsync(string threadId, string userId)
        {
            if (!await IsDeleteAuthorized(threadId, userId)) {
                throw new Exception("User does not have permission to delete comment.");
            }

            await this.commentRepository.HardDeleteAllThreadCommentsAsync(threadId);
            await this.threadRepository.DeleteThreadAsync(threadId);
        }
    }
}
