using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class SpoolService
    {
        private readonly SpoolRepository spoolRepository;
        public SpoolService(PostgresDbContext context)
        {
            this.spoolRepository = new SpoolRepository(context);
        }

        public async Task<Spool?> GetSpoolAsync(string spoolId)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolAsync(spoolId);
            if (returnedSpool != null)
            {
                return returnedSpool;
            }
            else
            {
                throw new Exception("Spool does not exist.");
            }
        }

        public async Task<Spool?> GetSpoolAsync(Spool spool)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolAsync(spool);
            if (returnedSpool != null)
            {
                return returnedSpool;
            }
            else
            {
                throw new Exception("Spool does not exist.");
            }
        }

        public async Task<Spool?> GetSpoolByNameAsync(string spoolName)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolByNameAsync(spoolName);
            if (returnedSpool != null)
            {
                return returnedSpool;
            }
            else
            {
                throw new Exception("Spool does not exist.");
            }
        }

        public async Task<Spool> InsertSpoolAsync(Spool spool)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolByNameAsync(spool.Name);
            if (returnedSpool != null)
            {
                throw new Exception("Spool already exists.");
            }
            else
            {
                await this.spoolRepository.InsertSpoolAsync(spool);
                return spool!;
            }
        }

        public async Task<Spool?> AddModeratorAsync(string spoolId, string userName)
        {
            UserRepository userRepository = new UserRepository(new PostgresDbContext());

            Spool? currentSpool = await this.GetSpoolAsync(spoolId);
            if(currentSpool == null)
            {
                throw new Exception("Spool does not exist.");
            }
            UserDTO? spoolOwner = await userRepository.GetUserAsync(currentSpool!.OwnerId);
            if(spoolOwner == null)
            {
                throw new Exception("Error getting owner");
            }
            if(spoolOwner.Username == userName)
            {
                throw new Exception("Cannot add owner as moderator.");
            }
            UserDTO[]? mods = await this.spoolRepository.GetModeratorsAsync(spoolId);
            UserDTO? newMod = await userRepository.GetUserByLoginIdentifierAsync(userName);
            foreach (UserDTO mod in mods)
            {
                if (newMod != null && mod.Id == newMod.Id)
                {
                    throw new Exception("User is already a mod.");
                }
            }

            Spool? dbSpool = await this.spoolRepository.AddModeratorAsync(spoolId, userName);
            if(dbSpool != null)
            {
                return dbSpool;
            }
            else
            {
                throw new Exception("User does not exist.");
            }
        }

        public async Task<Spool?> ChangeOwnerAsync(string spoolId, string userName)
        {
            UserRepository userRepository = new UserRepository( new PostgresDbContext() );
            Spool? currentSpool = await spoolRepository.GetSpoolAsync(spoolId);
            if(currentSpool == null)
            {
                throw new Exception("Spool does not exist");
            }
            UserDTO? currentOwner = await userRepository.GetUserAsync(currentSpool!.OwnerId);
            UserDTO? newOwner = await userRepository.GetUserByLoginIdentifierAsync(userName);

            if (currentOwner != null && currentSpool != null && newOwner != null && currentOwner.Id == newOwner.Id)
            {
                throw new Exception("User is already the owner.");
            }

            Spool? dbSpool = await this.spoolRepository.ChangeOwnerAsync(spoolId, userName);
            if (dbSpool != null)
            {
                return dbSpool;
            }
            else
            {
                throw new Exception("User does not exist.");
            }
        }

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userId)
        {
            return await this.spoolRepository.RemoveModeratorAsync(spoolId, userId);
        }

        public async Task<Spool[]> GetAllSpoolsAsync()
        {
            Spool[] spools = await this.spoolRepository.GetAllSpoolsAsync();
            return spools;
        }

        public async Task<List<Spool>> GetJoinedSpoolsAsync(string userId)
        {
            List<Spool> spools = await this.spoolRepository.GetJoinedSpoolsAsync(userId);
            return spools;
        }

        public async Task<UserDTO[]?> GetAllModsForSpoolAsync(string spoolId)
        {
            UserDTO[]? users = await this.spoolRepository.GetModeratorsAsync(spoolId);
            return users;
        }

        public async Task DeleteSpoolAsync(string spoolId)
        {
            await this.spoolRepository.DeleteSpoolAsync(spoolId);
        }

        public async Task SaveRulesAsync(string spoolId, string rules)
        {
            await this.spoolRepository.SaveRulesAsync(spoolId, rules);
        }
    }
}
