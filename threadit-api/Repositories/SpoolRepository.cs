using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                        Console.WriteLine("id: " + currentId);
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

        public async Task<Spool?> AddModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
                return null;
            dbSpool.Moderators.Add(userId);
            await db.SaveChangesAsync();
            return dbSpool;
        }

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
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

        public async Task<UserDTO[]> GetAllNonModeratorsForSpoolAsync(string spoolId, string userId)
        {
            if(userId == null)
            {
                return Array.Empty<UserDTO>();
            }
            UserSettings[]? userSettings = await db.UserSettings.OrderBy(u => u.Id).ToArrayAsync();
            List<UserDTO> usersList = new List<UserDTO> { };
            UserDTO[]? moderators = await this.GetModeratorsAsync(spoolId);
            if(moderators == null)
            {
                moderators = Array.Empty<UserDTO>();
            }
            foreach (var setting in userSettings)
            {
                //if its not the spool owner
                if (!setting.Id.Equals(userId))
                {
                    //if this user is part of the spool
                    if (setting.SpoolsJoined.Contains(spoolId))
                    {
                        //if there are any moderators
                        if (moderators != null)
                        {
                            //if this user is not a moderator of the spool
                            if (!moderators.Any(mod => mod.Id.Equals(setting.Id)))
                            {
                                UserDTO? currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == setting.Id);
                                if (currentUser != null)
                                {
                                    usersList.Add(currentUser);
                                }
                            }
                        }
                        else
                        {
                            UserDTO? currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == setting.Id);
                            if (currentUser != null)
                            {
                                usersList.Add(currentUser);
                            }
                        }
                    }
                }
            }
            return usersList.ToArray();
        }

        public async Task<UserDTO[]> GetAllUsersForSpoolAsync(string spoolId, string userId)
        {
            if (userId == null)
            {
                return Array.Empty<UserDTO>();
            }
            UserSettings[]? userSettings = await db.UserSettings.OrderBy(u => u.Id).ToArrayAsync();
            List<UserDTO> usersList = new List<UserDTO> { };
            foreach (var setting in userSettings)
            {
                //if its not the spool owner
                if (!setting.Id.Equals(userId))
                {
                    //if this user is part of the spool
                    if (setting.SpoolsJoined.Contains(spoolId))
                    {
                        UserDTO? currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == setting.Id);
                        if (currentUser != null)
                        {
                            usersList.Add(currentUser);
                        }
                    }
                }
            }
            return usersList.ToArray();
        }
    }
}
