using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Constants;
using ThreaditAPI.Database;

namespace ThreaditAPI.Repositories
{
    public class ThreadRepository : AbstractRepository
    {
        const int PAGE_SIZE = 10;
        const double SECONDS_PER_DAY = 24 * 60 * 60;

        public ThreadRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Models.Thread?> GetThreadAsync(string threadId)
        {
            Models.Thread? dbThread = await db.Threads.FirstOrDefaultAsync(u => u.Id == threadId);
            return dbThread == default ? null : dbThread;
        }

        private IOrderedQueryable<Models.Thread> ApplyThreadSort(IQueryable<Models.Thread> query, string sort)
        {
            switch (sort)
            {
                case SortConstants.SORT_HOT:
                    return query.OrderByDescending(thread => thread.Stitches.Count / ((DateTime.UtcNow - thread.DateCreated).TotalSeconds / SECONDS_PER_DAY)).ThenByDescending(thread => thread.DateCreated);
                case SortConstants.SORT_TOP:
                    return query.OrderByDescending(thread => thread.Stitches.Count).ThenByDescending(thread => thread.DateCreated);
                case SortConstants.SORT_CONTROVERSIAL:
                    return query.OrderByDescending(thread => ((1 + (2.0 * thread.Rips.Count) - thread.Stitches.Count) / (1 + thread.Rips.Count + thread.Stitches.Count))).ThenByDescending(thread => thread.DateCreated);
                default:
                    return query.OrderByDescending(thread => thread.DateCreated);
            }
        }

        private IQueryable<Models.Thread> ApplyThreadSearch(IQueryable<Models.Thread> query, string search)
        {
            return query.Where(thread => thread.Topic.Contains(search) || thread.Title.Contains(search) || thread.Content.Contains(search));
        }

        public async Task<Models.Thread[]> GetThreadsAsync(string? sort = null, string? searchQuery = null, int? skip = null, string? spoolId = null)
        {
            if (sort == null) sort = SortConstants.SORT_NEW;

            IQueryable<Models.Thread> query = db.Threads;

            if (spoolId != null) query = query.Where(u => u.SpoolId == spoolId);
            query = ApplyThreadSort(query, sort);
            if (searchQuery != null) query = ApplyThreadSearch(query, searchQuery);
            if (skip != null) query = query.Skip((int)skip);
            query = query.Take(PAGE_SIZE);
            return await query.ToArrayAsync();
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

        public async Task DeleteThreadAsync(string threadId)
        {
            Models.Thread? originalThread = await GetThreadAsync(threadId);
            if (originalThread != null)
            {
                db.Threads.Remove(originalThread);
                await db.SaveChangesAsync();
            }
        }
    }
}
