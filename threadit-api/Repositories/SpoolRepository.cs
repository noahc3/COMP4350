using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class SpoolRepository : AbstractRepository
    {
        public SpoolRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Spool?> GetSpoolAsync(Spool spool)
        {
            return await GetSpoolAsync(spool.Id);
        }

        public async Task<Spool?> GetSpoolAsync(string spoolId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            return dbSpool == default ? null : dbSpool;
        }

        public async Task<Spool?> GetSpoolByNameAsync(string spoolName)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Name == spoolName);
            return dbSpool == default ? null : dbSpool;
        }

        public async Task InsertSpoolAsync(Spool spool)
        {
            await db.Spools.AddAsync(spool);
            await db.SaveChangesAsync();
        }

        public async Task<List<string>?> GetModeratorsAsync(string spoolId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool != null)
            {
                return dbSpool.Moderators.First().Split(',').ToList();
            }
            return null;
        }

        public async Task AddModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
                return;
            dbSpool.Moderators.Add(userId);
            await db.SaveChangesAsync();
        }

        public async Task<string?> RemoveModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
            {
                return null;
            }
            dbSpool.Moderators.Remove(userId);
            await db.SaveChangesAsync();
            return userId;
        }

        public async Task<Spool[]> GetAllSpoolsAsync()
        {
            Spool[] spools = await db.Spools.OrderBy(u => u.Name).ToArrayAsync();
            return spools;
        }

        public async Task<Spool[]> GetJoinedSpoolsAsync(string userId)
        {
            UserSettings? setting = await db.UserSettings.FirstOrDefaultAsync(u => u.Id == userId);
            List<string>? spoolIds = setting?.SpoolsJoined;

            Spool[] spools = Array.Empty<Spool>();
            if (spoolIds != null)
            {
                foreach (var id in spoolIds)
                {
                    Spool? dbSpool = await GetSpoolAsync(id);
                    if (dbSpool != null)
                    {
                        spools?.Append(dbSpool);
                    }
                }
            }
            Console.WriteLine(spools);
            return spools;
        }
    }
}
