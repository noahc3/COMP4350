using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class ModeratorProfileService
    {
        private readonly ModeratorRepository moderatorRepository;
        public ModeratorProfileService(PostgresDbContext context)
        {
            this.moderatorRepository = new ModeratorRepository(context);
        }

        public async Task<ModeratorProfile?> GetModeratorProfileAsync(string moderatorId)
        {
            return await this.moderatorRepository.GetModeratorAsync(moderatorId);
        }

        public async Task<ModeratorProfile?> GetModeratorProfileAsync(ModeratorProfile moderator)
        {
            return await this.moderatorRepository.GetModeratorAsync(moderator);
        }

        public async Task<ModeratorProfile> InsertModeratorProfileAsync(ModeratorProfile moderator)
        {
            await this.moderatorRepository.InsertModeratorAsync(moderator);
            return moderator;
        }

    }
}
