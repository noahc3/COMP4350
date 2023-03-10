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
            UserDTO? returnedUser = await this.userRepository.GetUserAsync(userId);
            if(returnedUser != null)
            {
                return returnedUser;
            }
            else
            {
                throw new Exception("User does not exist.");
            }
        }

        public async Task<UserDTO?> GetUserAsync(UserDTO user) {
            UserDTO? returnedUser = await this.userRepository.GetUserAsync(user);
            if (returnedUser != null)
            {
                return returnedUser;
            }
            else
            {
                throw new Exception("User does not exist.");
            }
        }

        public async Task<User?> GetUserAsync(string username, string password) {
            User? user = await this.userRepository.GetUserByLoginIdentifierAsync(username);
            if (user == null) {
                throw new Exception("User does not exist.");
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

        public async Task<UserDTO?> DeleteUserAsync(string userId)
        {
            UserDTO? user = await this.userRepository.DeleteUserAsync(userId);
            if (user == null)
            {
                throw new Exception("User does not exist.");
            }
            else
            {
                return user;
            }
        }
    }
}
