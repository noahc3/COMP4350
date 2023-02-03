using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories {
    public class UserSessionRepository : AbstractRepository {
        public UserSessionRepository(PostgresDbContext dbContext) : base(dbContext) {
        }

        public async Task<UserSession?> GetUserSessionAsync(UserSession session) {
            return await GetUserSessionAsync(session.Id);
        }

        public async Task<UserSession?> GetUserSessionAsync(string sessionId) {
            UserSession? dbSession = await db.UserSessions.FirstOrDefaultAsync(s => s.Id == sessionId);
            return dbSession == default ? null : dbSession;
        }

        public async Task InsertUserSessionAsync(UserSession session) {
            await db.UserSessions.AddAsync(session);
            await db.SaveChangesAsync();
        }

        public async Task DeleteUserSessionAsync(UserSession session) {
            db.UserSessions.Remove(session);
            await db.SaveChangesAsync();
        }
    }
}
