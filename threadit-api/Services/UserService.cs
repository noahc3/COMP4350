using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services {
    public class UserService {
        private readonly UserRepository userRepository;
        public UserService(PostgresDbContext context) {
            this.userRepository = new UserRepository(context);
        }

        public async Task<UserDTO?> GetUserAsync(string userId) {
            return await this.userRepository.GetUserAsync(userId);
        }

        public async Task<UserDTO?> GetUserAsync(UserDTO user) {
            return await this.userRepository.GetUserAsync(user);
        }

        public async Task<User?> GetUserAsync(string username, string password) {
            User? user = await this.userRepository.GetUserByLoginIdentifierAsync(username);
            if (user == null) {
                return null;
            }

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }

        public async Task<User> CreateUserAsync(string username, string email, string password) {
            if (await userRepository.GetUserByLoginIdentifierAsync(username) != null || await userRepository.GetUserByLoginIdentifierAsync(email) != null) {
                throw new Exception("Username or email already exists.");
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            User user = new User() {
                Username = username, 
                Email = email,
                PasswordHash = hash
            }; 

            await this.userRepository.InsertUserAsync(user);
            return user;
        }

        public async Task<UserDTO?> DeleteUserAsync(string username)
        {
            UserDTO? user = await this.userRepository.DeleteUserAsync(username);
            return user;
        }
    }
}
