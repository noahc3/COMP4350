using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class UserProfileRepository : AbstractRepository
    {
        public UserProfileRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserProfile?> GetUserProfileAsync(UserProfile userProfile)
        {
            return await GetUserProfileAsync(userProfile.Id);
        }

        public async Task<UserProfile?> GetUserProfileAsync(string userProfileId)
        {
            UserProfile? dbUserProfile = await db.UserProfiles.FirstOrDefaultAsync(u => u.Id == userProfileId);
            return dbUserProfile == default ? null : dbUserProfile;
        }

        public async Task InsertUserProfileAsync(UserProfile userProfile)
        {
            await db.UserProfiles.AddAsync(userProfile);
            await db.SaveChangesAsync();
        }
    }
}
