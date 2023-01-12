using BudgeteerAPI.Constants;
using BudgeteerAPI.Database;
using BudgeteerAPI.Models;
using BudgeteerAPI.Repositories;
using System.Security.Claims;

namespace BudgeteerAPI.Services {
    public class UserService {
        private readonly UserRepository userRepository;
        public UserService(BudgeteerDbContext context) {
            this.userRepository = new UserRepository(context);
        }

        public async Task<User?> GetUser(string userId) {
            return await this.userRepository.GetUser(userId);
        }

        public async Task<User?> GetUser(User user) {
            return await this.userRepository.GetUser(user);
        }

        public async Task<User> CreateUser() {
            User user = new User();
            await this.userRepository.InsertUser(user);
            return user;
        }

        public async Task<User> GetOrCreateUserAsync(UserAuthLinkService ualService, string authId, AuthSource source)
        {
            User? user = null;
            UserAuthLink? ual = await ualService.GetUserAuthLinkAsync(authId, source);

            if (ual == null) {
                user = await CreateUser();
                await ualService.AddUserAuthLink(user.Id, authId, source);
            } else {
                user = await GetUser(ual.UserId);
            }

            if (user == null)
            {
                throw new Exception("A valid user auth link was found but the user ID does not correspond to an existing user in the database.");
            }

            return user;
        }

    }
}
