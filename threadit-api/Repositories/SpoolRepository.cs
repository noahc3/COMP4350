using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Linq;

namespace ThreaditAPI.Repositories
{
    public class SpoolRepository : AbstractRepository
    {
        public SpoolRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Spool?> GetSpoolAsync(Spool spool)
        {
            return await GetSpoolAsync(spool.Id);
        }

        public async Task<Spool?> GetSpoolAsync(string spoolId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            return dbSpool == default ? null : dbSpool;
        }

        public async Task<Spool?> GetSpoolByNameAsync(string spoolName)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Name == spoolName);
            return dbSpool == default ? null : dbSpool;
        }

        public async Task InsertSpoolAsync(Spool spool)
        {
            //add spool to spools table
            await db.Spools.AddAsync(spool);

            //add spool to users joined list
            UserSettings? setting = await db.UserSettings.FirstOrDefaultAsync(u => u.Id == spool.OwnerId);

            if(setting == null)
            {
                throw new Exception("User Does not have settings.");
            }
            setting.SpoolsJoined.Add(spool.Id);

            await db.SaveChangesAsync();
        }

        public async Task<UserDTO[]?> GetModeratorsAsync(string spoolId)
        {
            UserDTO[] users = Array.Empty<UserDTO>();
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool != null)
            {
                if (!dbSpool.Moderators.IsNullOrEmpty())
                {
                    foreach (string currentId in dbSpool.Moderators)
                    {
                        UserDTO? currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == currentId);
                        if (currentUser != null)
                        {
                            Array.Resize(ref users, users.Length + 1);
                            users[^1] = currentUser;
                        }
                    }
                }
            }
            return users;
        }

        public async Task<Spool?> AddModeratorAsync(string spoolId, string userName)
        {
            Spool? dbSpool = await GetSpoolAsync(spoolId);
            UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (dbSpool == null || dbUser == null|| dbSpool.Moderators.Contains(dbUser.Id))
                return null;
            dbSpool.Moderators.Add(dbUser.Id);
            await db.SaveChangesAsync();
            return dbSpool;
        }

        public async Task<Spool?> ChangeOwnerAsync(string spoolId, string userName)
        {
            Spool? dbSpool = await GetSpoolAsync(spoolId);
            UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (dbSpool == null || dbUser == null)
            {
                return null;
            }
            dbSpool!.OwnerId = dbUser!.Id;
            await db.SaveChangesAsync();
            return dbSpool;
        }

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userName)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (dbSpool == null || dbUser == null)
            {
                return null;
            }
            dbSpool.Moderators.Remove(dbUser.Id);
            await db.SaveChangesAsync();
            return dbSpool;
        }

        public async Task<Spool[]> GetAllSpoolsAsync()
        {
            Spool[] spools = await db.Spools.OrderBy(u => u.Name).ToArrayAsync();
            return spools;
        }

        public async Task<List<Spool>> GetJoinedSpoolsAsync(string userId)
        {
            UserSettings? setting = await db.UserSettings.FirstOrDefaultAsync(u => u.Id == userId);
            List<string>? spoolIds = setting?.SpoolsJoined;

            List<Spool> spools = new List<Spool>();
            if (spoolIds != null)
            {
                foreach (var id in spoolIds)
                {
                    Spool? dbSpool = await GetSpoolAsync(id);
                    if (dbSpool != null)
                    {
                        spools.Add(dbSpool);
                    }
                }
            }
            return spools;
        }

        public async Task DeleteSpoolAsync(string spoolId)
        {
            Spool? dbSpool = await GetSpoolAsync(spoolId);
            if (dbSpool != null)
            {
                db.Spools.Remove(dbSpool);
                await db.SaveChangesAsync();
            }
        }

        public async Task SaveRulesAsync(string spoolId, string rules)
        {
            Spool? dbSpool = await GetSpoolAsync(spoolId);
            if (dbSpool != null)
            {
                dbSpool.Rules = rules;
                await db.SaveChangesAsync();
            }
        }
    }
}
