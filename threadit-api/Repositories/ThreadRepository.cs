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

        public async Task InsertThreadAsync(Models.Thread thread)
        {
            await db.Threads.AddAsync(thread);
            await db.SaveChangesAsync();
        }
    }
}
