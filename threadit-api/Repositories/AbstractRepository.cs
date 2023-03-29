using ThreaditAPI.Database;

namespace ThreaditAPI.Repositories
{
	public class AbstractRepository
	{
		protected readonly PostgresDbContext db;

		public AbstractRepository(PostgresDbContext dbContext)
		{
			this.db = dbContext;
		}
	}
}
