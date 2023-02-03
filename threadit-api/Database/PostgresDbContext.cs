using ThreaditAPI.Constants;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Database {
    public class PostgresDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        public PostgresDbContext() {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            base.OnConfiguring(options);

            string host = ExternalServicesConstants.DB_HOST;
            string user = ExternalServicesConstants.DB_USER;
            string password = ExternalServicesConstants.DB_PASSWORD;
            string db = ExternalServicesConstants.DB_NAME;

            options.UseNpgsql($"Host={host};Username={user};Password={password};Database={db}");
        }
    }
}
