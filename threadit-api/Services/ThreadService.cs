using ThreaditAPI.Database;
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
                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = threads[i].Id,
                    Content = threads[i].Content,
                    Topic = threads[i].Topic,
                    Title = threads[i].Title,
                    SpoolId = threads[i].SpoolId,
                    OwnerId = threads[i].OwnerId,
                    DateCreated = threads[i].DateCreated,
                    AuthorName = (await new UserService(new PostgresDbContext()).GetUserAsync(threads[i].OwnerId)).Username,
                    SpoolName = (await new SpoolService(new PostgresDbContext()).GetSpoolAsync(threads[i].SpoolId)).Name
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
                Models.ThreadFull fullThread = new Models.ThreadFull() {
                    Id = threads[i].Id,
                    Content = threads[i].Content,
                    Topic = threads[i].Topic,
                    Title = threads[i].Title,
                    SpoolId = threads[i].SpoolId,
                    OwnerId = threads[i].OwnerId,
                    DateCreated = threads[i].DateCreated,
                    AuthorName = (await new UserService(new PostgresDbContext()).GetUserAsync(threads[i].OwnerId)).Username,
                    SpoolName = (await new SpoolService(new PostgresDbContext()).GetSpoolAsync(threads[i].SpoolId)).Name
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
    }
}
