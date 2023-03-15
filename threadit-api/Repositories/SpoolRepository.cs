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
            //verify the mods exist
            List<string> moderatorIds = spool.Moderators;
            List<string> modsToRemove = new List<string>();
            foreach (var moderatorId in moderatorIds)
            {
                UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Id == moderatorId);
                if(dbUser == null)
                {
                    modsToRemove.Add(moderatorId);
                }
                else
                {
                    //add spool to users joined list for the mod
                    UserSettings? modSettings = await db.UserSettings.FirstOrDefaultAsync(u => u.Id == moderatorId);
                    modSettings?.SpoolsJoined.Add(spool.Id);
                }
            }
            
            foreach(var moderatorId in modsToRemove)
            {
                spool.Moderators.Remove(moderatorId);
            }

            //add spool to spools table
            await db.Spools.AddAsync(spool);

            //add spool to users joined list for the owner
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
            if (dbSpool == null || dbUser == null)
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

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (dbSpool == null)
            {
                return null;
            }
            if (dbUser == null && !dbSpool.Moderators.Contains(userId))
            {
                return null;
            }
            dbSpool.Moderators.Remove(userId);
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
                Models.Thread[] dbThreads = await db.Threads.Where(u => u.SpoolId == spoolId).ToArrayAsync();
                foreach (var dbThread in dbThreads)
                {
                    db.Threads.Remove(dbThread);
                }
                UserDTO[] spoolUsers = await GetUsersForSpool(spoolId);
                UserSettingsRepository userSettingsRepository = new UserSettingsRepository( new PostgresDbContext() );
                foreach (var spoolUser in spoolUsers)
                {
                    await userSettingsRepository.RemoveUserSettingsAsync(spoolUser.Id, dbSpool.Name);
                }
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

        private async Task<UserDTO[]> GetUsersForSpool(string spoolId)
        {
            UserDTO[] users = Array.Empty<UserDTO>();
            UserSettings[] allUsers = await db.UserSettings.ToArrayAsync();

            foreach (var user in allUsers)
            {
                if (user.SpoolsJoined.Contains(spoolId))
                {
                    UserDTO? currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                    if (currentUser != null)
                    {
                        Array.Resize(ref users, users.Length + 1);
                        users[^1] = currentUser;
                    }
                }
            }
            return users;
        }
    }
}
