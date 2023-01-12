using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using BudgeteerAPI.Repositories;

namespace BudgeteerAPI.Services
{
    public class UserAuthLinkService
    {
        private readonly UserAuthLinkRepository ualRepository;
        public UserAuthLinkService(BudgeteerDbContext context)
        {
            this.ualRepository = new UserAuthLinkRepository(context);
        }

        public async Task<UserAuthLink?> GetUserAuthLinkAsync(string authId, AuthSource authSource)
        {
            return await this.ualRepository.GetUserAuthLink(authId, authSource);
        }

        public async Task<UserAuthLink> AddUserAuthLink(string userId, string authId, AuthSource authSource)
        {
            UserAuthLink ual = new UserAuthLink() { AuthId = authId, AuthSource = authSource, UserId = userId };
            await ualRepository.InsertUserAuthLink(ual);
            return ual;
        }

    }
}
