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
            return await this.userSettingsRepository.GetUserSettingsAsync(userSettingsId);
        }

        public async Task<UserSettings?> GetUserSettingsAsync(UserSettings userSettings)
        {
            return await this.userSettingsRepository.GetUserSettingsAsync(userSettings);
        }

        public async Task<UserSettings> InsertUserSettingsAsync(UserSettings userSettings)
        {
            await this.userSettingsRepository.InsertUserSettingsAsync(userSettings);
            return userSettings;
        }

    }
}
