using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;
using ThreaditAPI.Models;

namespace ThreaditAPI.Repositories
{
	public class UserRepository : AbstractRepository
	{
		public UserRepository(PostgresDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<UserDTO?> GetUserAsync(UserDTO user)
		{
			return await GetUserAsync(user.Id);
		}

		public async Task<UserDTO?> GetUserAsync(string userId)
		{
			UserDTO? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
			return dbUser == default ? null : dbUser;
		}

		public async Task<User?> GetUserByLoginIdentifierAsync(string loginIdentifier)
		{
			User? dbUser = await db.Users.FirstOrDefaultAsync(u => u.Username == loginIdentifier || u.Email == loginIdentifier);
			return dbUser == default ? null : dbUser;
		}

		public async Task InsertUserAsync(User user)
		{
			UserSettings userSettings = new UserSettings
			{
				Id = user.Id
			};
			await db.Users.AddAsync(user);
			await db.UserSettings.AddAsync(userSettings);
			await db.SaveChangesAsync();
		}

		public async Task<UserDTO?> DeleteUserAsync(string userId)
		{
			UserDTO? returnUserDTO = await GetUserAsync(userId);
			User? returnUser = null;
			if (returnUserDTO != null)
			{
				returnUser = await GetUserByLoginIdentifierAsync(returnUserDTO!.Username);
			}

			if (returnUser == null) return null;

			db.Users.Remove(returnUser);
			await db.SaveChangesAsync();
			return returnUser;
		}
	}
}
