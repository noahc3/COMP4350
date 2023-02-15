using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class SpoolRepositoryTests
{
    private SpoolRepository _spoolRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _spoolRepository = new SpoolRepository(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task RetrieveSpool_NotExists_ShouldFail()
    {
        Spool spool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "doesNotExistSpool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
        };
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(spool);
        
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task RetrieveSpoolById_NotExists_ShouldFail()
    {
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task RetrieveSpool_Exists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>(){"Interest1", "Interest2"}
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
    }

    [Test]
    public async Task RetrieveSpoolById_Exists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>(){"Interest1", "Interest2"}
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
    }

    [Test]
    public async Task AddModerator_SpoolExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators= new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //add moderator
        await _spoolRepository.AddModeratorAsync(testSpool.Id, "858ebb7f-9b4a-41d7-9dd2-4814cd66607a");

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        //make sure thread has been updated properly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
        Assert.IsTrue(returnedSpool.Moderators.Contains("858ebb7f-9b4a-41d7-9dd2-4814cd66607a"));
    }

    [Test]
    public async Task RemoveModerator_SpoolExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //remove moderator
        string? removedUserId = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, "923f3675-90e5-458f-a997-73f263d01f95");

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        //make sure thread has been updated properly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
        Assert.IsFalse(returnedSpool.Moderators.Contains("923f3675-90e5-458f-a997-73f263d01f95"));
    }

    [Test]
    public async Task AddModerators_SpoolNotExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //add moderator
        await _spoolRepository.AddModeratorAsync(testSpool.Id, "858ebb7f-9b4a-41d7-9dd2-4814cd66607a");

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task RemoveModerators_SpoolNotExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //add moderator
        string? removedUserId = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, "923f3675-90e5-458f-a997-73f263d01f95");

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task GetModerators_SpoolExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //retrieve moderators
        List<string>? mods = await _spoolRepository.GetModeratorsAsync(testSpool.Id);

        //make sure the list is the same
        Assert.False(mods.IsNullOrEmpty());
        Assert.IsTrue(returnedSpool.Moderators.Contains("16f0fdb8-8af3-4f9c-b19a-b4a4277692bd"));
        Assert.IsTrue(returnedSpool.Moderators.Contains("923f3675-90e5-458f-a997-73f263d01f95"));
    }

    [Test]
    public async Task GetModerators_SpoolNotExists_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd", "923f3675-90e5-458f-a997-73f263d01f95" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //retrieve moderators
        List<string>? mods = await _spoolRepository.GetModeratorsAsync(testSpool.Id);

        //checks
        Assert.True(mods.IsNullOrEmpty());
    }
}