using BudgeteerAPI.Database;

namespace BudgeteerAPI.Repositories {
    public class AbstractRepository {
        protected readonly BudgeteerDbContext db;

        public AbstractRepository(BudgeteerDbContext dbContext) {
            this.db = dbContext;
        }
    }
}
