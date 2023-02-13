using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories {
    public class UserRepository : AbstractRepository {
        public UserRepository(PostgresDbContext dbContext) : base(dbContext) {
        }

        public async Task<UserDTO?> GetUserAsync(UserDTO user) {
            return await GetUserAsync(user.Id);
        }

        public async Task<UserDTO?> GetUserAsync(string userId) {
            UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return dbUser == default ? null : dbUser;
        }

        public async Task<User?> GetUserByLoginIdentifierAsync(string loginIdentifier) {
            User? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == loginIdentifier || u.Email == loginIdentifier);
            return dbUser == default ? null : dbUser;
        }

        public async Task InsertUserAsync(User user) {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }

        public async Task<UserDTO?> DeleteUserAsync(string userId)
        {
            UserDTO? returnUser = await GetUserAsync(userId);

            if(returnUser == null) return null;

            string sqlQuery = "DELETE FROM \"Users\" WHERE \"Id\"='" + userId + "';";
            db.Database.ExecuteSqlRaw(sqlQuery);
            return returnUser;
        }
    }
}
