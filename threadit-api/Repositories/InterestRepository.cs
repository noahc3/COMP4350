using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories {
    public class InterestRepository : AbstractRepository {
        public InterestRepository(PostgresDbContext dbContext) : base(dbContext) {
        }

        public async Task<Interest[]> GetAllInterestsAsync(){
            Interest[] interests = await db.Interests.ToArrayAsync();
            return interests;
        }
    }
}
