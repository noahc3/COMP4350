using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
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
    public void RetrieveSpoolByName_NotExists_ShouldFail()
    {
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync("doesNotExistSpool"));
    }

    [Test]
    public async Task RetrieveSpoolByName_Exists_ShouldPass() {
        // Create Spool
        var spool = new Spool()
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
        Assert.That(retrievedSpool!.Id, Is.EqualTo(spool.Id));
        Assert.That(retrievedSpool.Name, Is.EqualTo(spool.Name));
        Assert.That(retrievedSpool.OwnerId, Is.EqualTo(spool.OwnerId));
    }

    [Test]
    public async Task RetrieveAllSpools_ShouldPass()
    {
        // Create Spools
        var spools = new List<Spool>() {
            new Spool() {
                Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                Name = "Spool1",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            },
            new Spool() {
                Id = "adadada-9031-4e9b-b712-6df32cd75641",
                Name = "Spool2",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            }
        };

        //add them to the db
        foreach (var spool in spools)
        {
            await _spoolService.InsertSpoolAsync(spool);
        }

        Spool[] returnedSpools = await _spoolService.GetAllSpoolsAsync();

        Assert.False(returnedSpools.IsNullOrEmpty());
        Assert.True(returnedSpools[0].Id.Equals("qr5t9c51-9031-4e9b-b712-6df32cd75641"));
        Assert.True(returnedSpools[0].Name.Equals("Spool1"));
        Assert.True(returnedSpools[1].Id.Equals("adadada-9031-4e9b-b712-6df32cd75641"));
        Assert.True(returnedSpools[1].Name.Equals("Spool2"));
    }
}