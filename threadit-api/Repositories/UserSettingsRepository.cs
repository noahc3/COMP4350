using System.Linq;
using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;
using ThreaditAPI.Models;

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

            string[] interests = dbSpool.Interests.ToArray();
            foreach (string inter in interests)
            {
                await this.RemoveUserInterestAsync(userId, inter);
            }
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
                string[] interests = dbSpool.Interests.ToArray();
                foreach (string inter in interests)
                {
                    await this.AddUserInterestAsync(userId, inter);
                }
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

        // public async Task<string[]> GetUserInterestsAsync(string userId)
        // {
        //     UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
        //     if (resultSettings == null)
        //         throw new Exception("Settings do not exist.");

        //     string[] interests = resultSettings.Interests.ToArray();
        //     return interests;
        // }

        public async Task<string[]> AddUserInterestAsync(string userId, string interest)
        {
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            if (resultSettings == null)
                throw new Exception("Settings do not exist.");

            if (!resultSettings.Interests.Contains(interest))
            {
                resultSettings.Interests.Add(interest);
                await db.SaveChangesAsync();
            }

            string[] interests = resultSettings.Interests.ToArray();
            return interests;
        }

        public async Task<string[]> RemoveUserInterestAsync(string userId, string interest)
        {
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            if (resultSettings == null)
                throw new Exception("Settings do not exist.");

            if (resultSettings.Interests.Contains(interest))
            {
                resultSettings.Interests.Remove(interest);
                await db.SaveChangesAsync();
            }

            string[] interests = resultSettings.Interests.ToArray();
            return interests;
        }

        public async Task<bool> BelongInterestAsync(string userId, string interest)
        {
            UserSettings? resultSettings = await this.GetUserSettingsAsync(userId);
            bool result = false;
            if (resultSettings == null)
                throw new Exception("settings do not exist.");

            if (resultSettings.Interests.Contains(interest))
            {
                result = true;
            }
            return result;
        }
    }
}
