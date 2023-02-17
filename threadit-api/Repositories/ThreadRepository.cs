using ThreaditAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class ThreadRepository : AbstractRepository
    {
        public ThreadRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Models.Thread?> GetThreadAsync(Models.Thread thread)
        {
            return await GetThreadAsync(thread.Id);
        }

        public async Task<Models.Thread?> GetThreadAsync(string threadId)
        {
            Models.Thread? dbThread = await db.Threads.FirstOrDefaultAsync(u => u.Id == threadId);
            return dbThread == default ? null : dbThread;
        }

        public async Task<Models.Thread[]> GetThreadsBySpoolAsync(string spoolId)
        {
            Models.Thread[] dbThreads = await db.Threads.Where(u => u.SpoolId == spoolId).OrderByDescending(u => u.DateCreated).ToArrayAsync();
            return dbThreads;
        }

        public async Task<Models.Thread[]> GetAllThreadsAsync()
        {
            Models.Thread[] dbThreads = await db.Threads.OrderByDescending(u => u.DateCreated).ToArrayAsync();
            return dbThreads;
        }

        public async Task InsertThreadAsync(Models.Thread thread)
        {
            await db.Threads.AddAsync(thread);
            await db.SaveChangesAsync();
        }

        public async Task UpdateThreadAsync(Models.Thread thread)
        {
            Models.Thread? originalThread = await GetThreadAsync(thread.Id);
            if (originalThread != null)
            {
                originalThread.Content = thread.Content;
                originalThread.Topic = thread.Topic;
                originalThread.Title = thread.Title;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteThreadAsync(string threadId) {
            Models.Thread? originalThread = await GetThreadAsync(threadId);
            if (originalThread != null)
            {
                db.Threads.Remove(originalThread);
                await db.SaveChangesAsync();
            }
        }
    }
}
