using ThreaditAPI.Constants;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Database {
    public class PostgresDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Models.Thread> Threads { get; set; }
        public DbSet<Spool> Spools { get; set; }
        public DbSet<Comment> Comments { get; set; }

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UserSeed
            modelBuilder.Entity<User>().HasData(new User { Id = "00000000-0000-456a-b0f7-7a8c172c23e0", Email = "test@gmail.com", Username = "testAccount", PasswordHash = "testPassword"});
            #endregion

            #region SpoolSeed
            modelBuilder.Entity<Spool>().HasData(new Spool { Id = "7f527ccf-a2bc-4adb-a7da-970be1175525", Name = "First Spool Ever!!!", Interests = "Hockey", OwnerId = "00000000-0000-456a-b0f7-7a8c172c23e0" });
            #endregion
        }
    }
}
