using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BudgeteerAPI.Repositories {
    public class UserAuthLinkRepository : AbstractRepository {
        public UserAuthLinkRepository(BudgeteerDbContext dbContext) : base(dbContext) {
        }

        public async Task<UserAuthLink?> GetUserAuthLink(string authId, AuthSource source) {
            UserAuthLink? dbUal = await db.UserAuthLinks.FirstOrDefaultAsync(ual => ual.AuthSource == source && ual.AuthId == authId);
            return dbUal == default ? null : dbUal;
        }

        public async Task InsertUserAuthLink(UserAuthLink ual) {
            await db.UserAuthLinks.AddAsync(ual);
            await db.SaveChangesAsync();
        }
    }
}
 