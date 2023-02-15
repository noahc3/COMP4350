using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services {
    public class UserSessionService {
        private readonly UserSessionRepository sessionRepository;
        private readonly UserRepository userRepository;
        public UserSessionService(PostgresDbContext context) {
            this.sessionRepository = new UserSessionRepository(context);
            this.userRepository = new UserRepository(context);
        }

        public async Task<UserSession?> GetUserSessionAsync(string sessionId) {
            UserSession? userSess = await this.sessionRepository.GetUserSessionAsync(sessionId);
            if (userSess != null)
            {
                return userSess;
            }
            else
            {
                throw new Exception("UserSession does not exist.");
            }
        }

        public async Task<UserSession?> GetUserSessionAsync(UserSession session) {
            UserSession? userSess = await this.sessionRepository.GetUserSessionAsync(session);
            if (userSess != null)
            {
                return userSess;
            }
            else
            {
                throw new Exception("UserSession does not exist.");
            }
        }

        public async Task<UserSession> CreateUserSessionAsync(User user) {
            UserSession session = new UserSession() {
                UserId = user.Id
            };

            await this.sessionRepository.InsertUserSessionAsync(session);
            return session;
        }

        public async Task<UserDTO?> GetUserFromSession(string sessionId) {
            UserSession? session = await this.sessionRepository.GetUserSessionAsync(sessionId);
            if (session == null) {
                return null;
            } else if (session.DateExpires < DateTime.Now) {
                await this.sessionRepository.DeleteUserSessionAsync(session);
                return null;
            }

            return await this.userRepository.GetUserAsync(session.UserId);
        }

    }
}
