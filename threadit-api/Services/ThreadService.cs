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
            return await this.threadRepository.GetThreadAsync(threadId);
        }

        public async Task<Models.Thread?> GetThreadAsync(Models.Thread thread)
        {
            return await this.threadRepository.GetThreadAsync(thread);
        }

        public async Task<Models.Thread> InsertThreadAsync(Models.Thread thread)
        {
            await this.threadRepository.InsertThreadAsync(thread);
            return thread;
        }

    }
}
