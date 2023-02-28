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

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userName)
        {
            return await this.spoolRepository.RemoveModeratorAsync(spoolId, userName);
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
