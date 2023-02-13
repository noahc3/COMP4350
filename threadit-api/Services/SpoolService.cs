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
            return await this.spoolRepository.GetSpoolAsync(spoolId);
        }

        public async Task<Spool?> GetSpoolAsync(Spool spool)
        {
            return await this.spoolRepository.GetSpoolAsync(spool);
        }

        public async Task<Spool> InsertSpoolAsync(Spool spool)
        {
            await this.spoolRepository.InsertSpoolAsync(spool);
            return spool;
        }

        public async Task<List<string>> GetModeratorsAsync(string spoolId)
        {
            List<string> moderators = await this.spoolRepository.GetModeratorsAsync(spoolId);
            return moderators;
        }

        public async Task AddModeratorAsync(string spoolId, string userId)
        {
            await this.spoolRepository.AddModeratorAsync(spoolId, userId);
        }

        public async Task RemoveModeratorAsync(string spoolId, string userId)
        {
            await this.spoolRepository.RemoveModeratorAsync(spoolId, userId);
        }
    }
}
