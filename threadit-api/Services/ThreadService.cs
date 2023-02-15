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
