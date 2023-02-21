using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class UserSettingsService
    {
        private readonly UserSettingsRepository userSettingsRepository;
        public UserSettingsService(PostgresDbContext context)
        {
            this.userSettingsRepository = new UserSettingsRepository(context);
        }

        public async Task<UserSettings?> GetUserSettingsAsync(string userSettingsId)
        {
            UserSettings? userSettings = await this.userSettingsRepository.GetUserSettingsAsync(userSettingsId);
            if (userSettings != null)
            {
                return userSettings;
            }
            else
            {
                throw new Exception("UserSettings does not exist.");
            }
        }

        public async Task<UserSettings?> GetUserSettingsAsync(UserSettings userSettings)
        {
            UserSettings? userSett = await this.userSettingsRepository.GetUserSettingsAsync(userSettings);
            if (userSett == null)
            {
                throw new Exception("UserSettings does not exist.");
            }
            return userSett;
        }

        public async Task<UserSettings> InsertUserSettingsAsync(UserSettings userSettings)
        {
            await this.userSettingsRepository.InsertUserSettingsAsync(userSettings);
            return userSettings;
        }

        public async Task<UserSettings> RemoveUserSettingsAsync(string userId, string spoolName)
        {
            UserSettings? resultSettings = await this.userSettingsRepository.RemoveUserSettingsAsync(userId, spoolName);
            return resultSettings;
        }

        public async Task<bool> CheckSpoolUserAsync(string userId, string spoolName)
        {
            return await this.userSettingsRepository.CheckSpoolUserAsync(userId, spoolName);
        }
    }
}
