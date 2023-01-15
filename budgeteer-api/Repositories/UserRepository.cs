using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BudgeteerAPI.Repositories {
    public class UserRepository : AbstractRepository {
        public UserRepository(BudgeteerDbContext dbContext) : base(dbContext) {
        }

        public async Task<User?> GetUserAsync(User user) {
            return await GetUserAsync(user.Id);
        }

        public async Task<User?> GetUserAsync(string userId) {
            User? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return dbUser == default ? null : dbUser;
        }

        public async Task InsertUserAsync(User user) {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
