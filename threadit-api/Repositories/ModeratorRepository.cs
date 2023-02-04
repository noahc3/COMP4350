using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class ModeratorRepository : AbstractRepository
    {
        public ModeratorRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ModeratorProfile?> GetModeratorAsync(ModeratorProfile moderator)
        {
            return await GetModeratorAsync(moderator.Id);
        }

        public async Task<ModeratorProfile?> GetModeratorAsync(string moderatorId)
        {
            ModeratorProfile? dbModerator = await db.Moderators.FirstOrDefaultAsync(u => u.Id == moderatorId);
            return dbModerator == default ? null : dbModerator;
        }

        public async Task InsertModeratorAsync(ModeratorProfile moderator)
        {
            await db.Moderators.AddAsync(moderator);
            await db.SaveChangesAsync();
        }
    }
}
