using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;
using ThreaditAPI.Models;

namespace ThreaditAPI.Repositories
{
    public class InterestRepository : AbstractRepository
    {
        public InterestRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Interest[]> GetAllInterestsAsync()
        {
            Interest[] interests = await db.Interests.ToArrayAsync();
            return interests;
        }

        public async Task<Interest[]> AddInterestAsync(string interestName)
        {
            //Adds to the interests table if it doesn't exist, iterates SpoolCount if it does
            if (!await db.Interests.AnyAsync(i => i.Name == interestName))
            {
                Interest interest = new Interest
                {
                    Name = interestName,
                    SpoolCount = 1
                };
                await db.Interests.AddAsync(interest);
                await db.SaveChangesAsync();
                Interest[] interestList = await db.Interests.ToArrayAsync();
                return interestList;
            }
            else
            {
                Interest interest = (await db.Interests.FirstOrDefaultAsync(i => i.Name == interestName))!;
                interest.SpoolCount++;
                await db.SaveChangesAsync();
                Interest[] interestList = await db.Interests.ToArrayAsync();
                return interestList;
            }
        }

        public async Task<int> DeIterateSpoolCount(Interest interest)
        {
            //If spoolCount is 0, that means there are no more threads with this interest, so it should be removed from the interests table
            interest.SpoolCount--;
            await db.SaveChangesAsync();
            return interest.SpoolCount;
        }

        public async Task<Interest[]> RemoveInterestAsync(string interestName)
        {
            //Removes from the interests table if it has no more Spools referencing this interest, deiterates SpoolCount if it does
            Interest? interest = await db.Interests.FirstOrDefaultAsync(i => i.Name == interestName);
            if (interest == null)
                throw new Exception("Interest does not exist");
            int spoolCount = await DeIterateSpoolCount(interest);
            if (spoolCount <= 0 && interestName != "General")
            {
                db.Interests.Remove(interest);
                await db.SaveChangesAsync();
            }
            Interest[] interestList = await db.Interests.ToArrayAsync();
            return interestList;
        }
    }
}
