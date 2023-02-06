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

        public async Task<User?> GetUserByLoginIdentifierAsync(string username) {
            User? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == username);
            return dbUser == default ? null : dbUser;
        }

        public async Task InsertUserAsync(User user) {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
