using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<UserSettings> RemoveUserSettingsAsync(string userId, string spoolName)
        {
            //get the user settings that needs to be edited
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            if (resultSettings == null)
            {
                throw new Exception("User Does not have settings.");
            }

            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Name == spoolName);
            if (dbSpool == null)
                throw new Exception("Spool does not exist");

            resultSettings.SpoolsJoined.Remove(dbSpool!.Id);
            await db.SaveChangesAsync();

            return resultSettings;
        }

        public async Task<UserSettings> JoinUserSettingsAsync(string userId, string spoolName)
        {
            //get the user settings that needs to be edited
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            if (resultSettings == null)
            {
                throw new Exception("User Does not have settings.");
            }

            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Name == spoolName);
            if (dbSpool == null)
                throw new Exception("Spool does not exist");

            if (!resultSettings.SpoolsJoined.Contains(dbSpool!.Id))
            {
                resultSettings.SpoolsJoined.Add(dbSpool!.Id);
                await db.SaveChangesAsync();
            }

            return resultSettings;
        }

        public async Task<bool> CheckSpoolUserAsync(string userId, string spoolName)
        {
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            if (resultSettings == null)
                throw new Exception("Settings do not exist.");

            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Name == spoolName);
            if (dbSpool == null)
                throw new Exception("Spool does not exist");

            bool belongs = resultSettings.SpoolsJoined.Contains(dbSpool.Id);
            return belongs;
        }

        public async Task<string[]> GetAllInterestsAsync()
        {
            string[] interests = await db.Interests.Select(i => i.Name).ToArrayAsync();
            return interests;
        }
    }
}
