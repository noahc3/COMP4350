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

    }
}
