using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class UserSettingsRepository : AbstractRepository
    {
        public UserSettingsRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserSettings?> GetUserSettingsAsync(UserSettings userSettings)
        {
            return await GetUserSettingsAsync(userSettings.Id);
        }

        public async Task<UserSettings?> GetUserSettingsAsync(string userSettingsId)
        {
            UserSettings? dbUserSettings = await db.UserSettings.FirstOrDefaultAsync(u => u.Id == userSettingsId);
            return dbUserSettings == default ? null : dbUserSettings;
        }

        public async Task InsertUserSettingsAsync(UserSettings userSettings)
        {
            await db.UserSettings.AddAsync(userSettings);
            await db.SaveChangesAsync();
        }
    }
}
