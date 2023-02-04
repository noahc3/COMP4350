using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class UserProfileService
    {
        private readonly UserProfileRepository userProfileRepository;
        public UserProfileService(PostgresDbContext context)
        {
            this.userProfileRepository = new UserProfileRepository(context);
        }

        public async Task<UserProfile?> GetUserProfileAsync(string userProfileId)
        {
            return await this.userProfileRepository.GetUserProfileAsync(userProfileId);
        }

        public async Task<UserProfile?> GetUserProfileAsync(UserProfile userProfile)
        {
            return await this.userProfileRepository.GetUserProfileAsync(userProfile);
        }

        public async Task<UserProfile> InsertUserProfileAsync(UserProfile userProfile)
        {
            await this.userProfileRepository.InsertUserProfileAsync(userProfile);
            return userProfile;
        }

    }
}
