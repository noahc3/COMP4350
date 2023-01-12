using BudgeteerAPI.Constants;
using BudgeteerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgeteerAPI.Database {
    public class BudgeteerDbContext : DbContext {
        public DbSet<UserAuthLink> UserAuthLinks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public BudgeteerDbContext() {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            base.OnConfiguring(options);

            string host = ExternalServicesConstants.DBHOST;
            string user = ExternalServicesConstants.DBUSER;
            string password = ExternalServicesConstants.DBPASSWORD;
            string db = ExternalServicesConstants.DBNAME;

            options.UseNpgsql($"Host={host};Username={user};Password={password};Database={db}");
        }
    }
}
