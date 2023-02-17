using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class ThreadService
    {
        private readonly ThreadRepository threadRepository;
        public ThreadService(PostgresDbContext context)
        {
            this.threadRepository = new ThreadRepository(context);
        }

        public async Task<Models.Thread?> GetThreadAsync(string threadId)
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

        public async Task<Models.Thread?> GetThreadAsync(Models.Thread thread)
        {
            Models.Thread? returnedThread = await this.threadRepository.GetThreadAsync(thread);
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
                Models.Spool? spool = await new SpoolService(new PostgresDbContext()).GetSpoolAsync(returnedThread.SpoolId);
                if (spool == null)
                {
                    throw new Exception($"Spool {returnedThread.SpoolId} does not exist.");
                }

                UserDTO? user = await new UserService(new PostgresDbContext()).GetUserAsync(returnedThread.OwnerId);
                if (user == null)
                {
                    throw new Exception($"User {returnedThread.OwnerId} does not exist.");
                }
                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = returnedThread.Id,
                    Content = returnedThread.Content,
                    Topic = returnedThread.Topic,
                    Title = returnedThread.Title,
                    SpoolId = returnedThread.SpoolId,
                    OwnerId = returnedThread.OwnerId,
                    DateCreated = returnedThread.DateCreated,
                    AuthorName = user.Username,
                    SpoolName = spool.Name
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
            Models.Spool? spool = await new SpoolService(new PostgresDbContext()).GetSpoolByNameAsync(spoolName);

            if (spool == null)
            {
                throw new Exception("Spool does not exist.");
            }
            Models.Thread[] threads = await this.threadRepository.GetThreadsBySpoolAsync(spool.Id);
            List<Models.ThreadFull> fullThreads = new List<Models.ThreadFull>();
            for (int i = 0; i < threads.Length; i++)
            {
                UserDTO? user = await new UserService(new PostgresDbContext()).GetUserAsync(threads[i].OwnerId);

                if (user == null)
                {
                    throw new Exception($"User {threads[i].OwnerId} does not exist.");
                }

                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = threads[i].Id,
                    Content = threads[i].Content,
                    Topic = threads[i].Topic,
                    Title = threads[i].Title,
                    SpoolId = threads[i].SpoolId,
                    OwnerId = threads[i].OwnerId,
                    DateCreated = threads[i].DateCreated,
                    AuthorName = user.Username,
                    SpoolName = spool.Name
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
                UserDTO? user = await new UserService(new PostgresDbContext()).GetUserAsync(threads[i].OwnerId);
                Spool? spool = await new SpoolService(new PostgresDbContext()).GetSpoolAsync(threads[i].SpoolId);

                if (user == null)
                {
                    throw new Exception($"User {threads[i].OwnerId} does not exist.");
                }

                if (spool == null)
                {
                    throw new Exception($"Spool {threads[i].SpoolId} does not exist.");
                }

                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = threads[i].Id,
                    Content = threads[i].Content,
                    Topic = threads[i].Topic,
                    Title = threads[i].Title,
                    SpoolId = threads[i].SpoolId,
                    OwnerId = threads[i].OwnerId,
                    DateCreated = threads[i].DateCreated,
                    AuthorName = user.Username,
                    SpoolName = spool.Name
                };
                fullThreads[i] = fullThread;
            }

            return fullThreads;
        }

        public async Task<Models.Thread> InsertThreadAsync(Models.Thread thread)
        {
            await this.threadRepository.InsertThreadAsync(thread);
            return thread;
        }

        public async Task<Models.Thread> UpdateThreadAsync(Models.Thread thread)
        {
            await this.threadRepository.UpdateThreadAsync(thread);
            return thread;
        }

        public async Task DeleteThreadAsync(string threadId)
        {
            await this.threadRepository.DeleteThreadAsync(threadId);
        }
    }
}
