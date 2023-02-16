using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class SpoolServiceTests
{
    private SpoolService _spoolService;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _spoolService = new SpoolService(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task RetrieveSpoolByName_NotExists_ShouldFail()
    {
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync("doesNotExistSpool"));
    }

    [Test]
    public async Task RetrieveSpoolByName_Exists_ShouldPass() {
        // Create Spool
        var spool = new ThreaditAPI.Models.Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool1",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
        };

        // Ensure spool is not in database
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync(spool.Name));

        // Insert spool into database
        await _spoolService.InsertSpoolAsync(spool);

        // Retrieve spool from database
        var retrievedSpool = await _spoolService.GetSpoolByNameAsync(spool.Name);

        Assert.NotNull(retrievedSpool);
        Assert.That(retrievedSpool.Id, Is.EqualTo(spool.Id));
        Assert.That(retrievedSpool.Name, Is.EqualTo(spool.Name));
        Assert.That(retrievedSpool.OwnerId, Is.EqualTo(spool.OwnerId));
    }
}