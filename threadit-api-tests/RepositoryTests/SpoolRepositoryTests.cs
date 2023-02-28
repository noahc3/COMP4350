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
    private UserRepository _userRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _spoolRepository = new SpoolRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
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
    public async Task RetrieveSpoolByName_NotExists_ShouldFail()
    {
        Spool? returnedSpool = await _spoolRepository.GetSpoolByNameAsync("Spool");

        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task RetrieveSpool_Exists_ShouldPass()
    {
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
    }

    [Test]
    public async Task RetrieveSpoolById_Exists_ShouldPass()
    {
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
    }

    [Test]
    public async Task RetrieveSpoolByName_Exists_ShouldPass()
    {
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolByNameAsync(testSpool.Name);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
    }

    [Test]
    public async Task AddModerator_SpoolExists_ShouldPass()
    {
        // Create Users for owner, and moderators
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        //mod1
        User mod1 = new User()
        {
            Id = "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd",
            Username = "mod1",
            Email = "testUser1@test.com"
        };
        //mod2
        User mod2 = new User()
        {
            Id = "923f3675-90e5-458f-a997-73f263d01f95",
            Username = "mod2",
            Email = "testUser2@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(mod1);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);

        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod2.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(mod2);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod2.Username);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { mod1.Id, mod2.Id }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
        returnedSpool = null;

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //create and add moderator
        //new mod
        User newMod = new User()
        {
            Id = "14155310-4f06-4582-b433-5f3ca2cf3305",
            Username = "newMod",
            Email = "newMod@test.com"
        };
        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(newMod.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(newMod);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(newMod.Username);
        Assert.That(returnedUser, Is.Not.Null);
        returnedSpool = await _spoolRepository.AddModeratorAsync(testSpool.Id, newMod.Username);
        Assert.That(returnedSpool, Is.Not.Null);

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        //make sure thread has been updated properly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
        Assert.IsTrue(returnedSpool.Moderators.Contains(newMod.Id));
    }

    [Test]
    public async Task RemoveModerator_SpoolExists_ShouldPass()
    {
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);


        //mod1
        User mod1 = new User()
        {
            Id = "923f3675-90e5-458f-a997-73f263d01f95",
            Username = "mod2",
            Email = "testUser2@test.com"
        };

        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(mod1);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { mod1.Id }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //remove moderator
        Spool? removeReturnedSpool = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, mod1.Username);

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        //make sure thread has been updated properly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.That(removeReturnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
        Assert.IsFalse(returnedSpool.Moderators.Contains(mod1.Id));
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
    public async Task RemoveModerators_SpoolNotExists_ShouldBeNull()
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
        Spool? removeReturnedSpool = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, "923f3675-90e5-458f-a997-73f263d01f95");
        Assert.That(removeReturnedSpool, Is.Null);

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task GetModerators_SpoolExists_ShouldPass()
    {
        //owner
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        //mod1
        User mod1 = new User()
        {
            Id = "16f0fdb8-8af3-4f9c-b19a-b4a4277692bd",
            Username = "mod1",
            Email = "testUser1@test.com"
        };
        //mod2
        User mod2 = new User()
        {
            Id = "923f3675-90e5-458f-a997-73f263d01f95",
            Username = "mod2",
            Email = "testUser2@test.com"
        };

        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(mod1);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod1.Username);
        Assert.That(returnedUser, Is.Not.Null);

        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod2.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(mod2);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(mod2.Username);
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { mod1.Id, mod2.Id }
        };
        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));

        //retrieve moderators
        UserDTO[]? mods = await _spoolRepository.GetModeratorsAsync(testSpool.Id);

        //make sure the list is the same
        Assert.False(mods.IsNullOrEmpty());
        Assert.IsTrue(returnedSpool.Moderators.Contains(mod1.Id));
        Assert.IsTrue(returnedSpool.Moderators.Contains(mod2.Id));
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
        UserDTO[]? mods = await _spoolRepository.GetModeratorsAsync(testSpool.Id);

        //checks
        Assert.True(mods.IsNullOrEmpty());
    }
}