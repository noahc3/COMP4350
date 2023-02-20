using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;

public class CommonUtils {
    public static PostgresDbContext GetDbContext() {
        var context = new PostgresDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    [OneTimeTearDown]
    public void TearDown() {
        var context = new PostgresDbContext();
        context.Database.EnsureDeleted();
    }
}