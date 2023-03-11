using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class SpoolRepositoryTests
{
    private SpoolRepository _spoolRepository;
    private UserRepository _userRepository;
    private UserSettingsRepository _userSettingsRepository;
    private PostgresDbContext _dbContext;
    private ThreadRepository _threadRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _spoolRepository = new SpoolRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
        _userSettingsRepository = new UserSettingsRepository(_dbContext);
        _threadRepository = new ThreadRepository(_dbContext);
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
    public async Task InsertSpool_ModNotExists_ShouldPass()
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

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "d89945fa-8ced-4849-bda9-dee94456e804" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
        Assert.That(returnedSpool, Is.Null);
        // Add Spool to database
        try
        {
            await _spoolRepository.InsertSpoolAsync(testSpool);
            Assert.Fail();
        }
        catch
        {
            returnedSpool = await _spoolRepository.GetSpoolByNameAsync(testSpool.Name);
            Assert.That(returnedSpool, Is.Null);
        }
    }

    [Test]
    public async Task InsertSpool_ModUserSettingsNotExist_ShouldPass()
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
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "d89945fa-8ced-4849-bda9-dee94456e804" }
        };

        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
        Assert.That(returnedSpool, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool);
        returnedSpool = await _spoolRepository.GetSpoolByNameAsync(testSpool.Name);
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
            Username = "mod1",
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
        Spool? removeReturnedSpool = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, mod1.Id);

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
    public async Task RemoveModerator_ModeratorNotExists_ShouldPass()
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
            Username = "mod1",
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
            Moderators = new List<string>() { "923f3675-90e5-458f-a997-73f263d01f95" }
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

        //delete user that is mod
        await _userRepository.DeleteUserAsync(mod1.Id);

        //remove moderator that just isnt anywhere
        Spool? removeReturnedSpool = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, mod1.Id);

        //get the Spool again
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);

        //make sure spool has been updated properly
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.That(removeReturnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool!.Id.Equals(testSpool.Id));
        Assert.IsTrue(returnedSpool.Name.Equals(testSpool.Name));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testSpool.OwnerId));
        Assert.IsTrue(returnedSpool.Interests.Equals(testSpool.Interests));
        Assert.IsFalse(returnedSpool.Moderators.Contains(mod1.Id));

        //remove moderator
        removeReturnedSpool = await _spoolRepository.RemoveModeratorAsync(testSpool.Id, "0fc935bc-fa2e-4d7b-b986-2f5e0781e4df");

        //make sure spool has been updated properly
        Assert.That(removeReturnedSpool, Is.Null);
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

    [Test]
    public async Task ChangeOwner_SpoolAndOwnerExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        //create user2
        User testUser2 = new User()
        {
            Id = "5e5f4089-6f48-4f69-8bce-3af8dd9b5f02",
            Username = "testUser2",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser2 = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser2, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser2);
        returnedUser2 = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser2, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() {  }
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

        //change owner
        returnedSpool = await _spoolRepository.ChangeOwnerAsync(testSpool.Id, testUser2.Username);
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.OwnerId.Equals(testUser2.Id));
    }

    [Test]
    public async Task ChangeOwner_OwnerNotExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { }
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

        //change owner
        returnedSpool = await _spoolRepository.ChangeOwnerAsync(testSpool.Id, "fakeUser");
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task ChangeOwner_SpoolNotExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        //create user2
        User testUser2 = new User()
        {
            Id = "5e5f4089-6f48-4f69-8bce-3af8dd9b5f02",
            Username = "testUser2",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser2 = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser2, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser2);
        returnedUser2 = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser2, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { }
        };
        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //change owner
        returnedSpool = await _spoolRepository.ChangeOwnerAsync(testSpool.Id, testUser2.Username);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task SaveRules_SpoolExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules= "First Rules"
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
        Assert.IsTrue(returnedSpool.Rules.Equals(testSpool.Rules));

        //change and save rules
        string newRules = "Updated these!";
        await _spoolRepository.SaveRulesAsync(testSpool.Id, newRules);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Not.Null);
        Assert.IsTrue(returnedSpool.Rules.Equals(newRules));
    }

    [Test]
    public async Task SaveRules_SpoolNotExist_ShouldPass()
    {
         // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //change and save rules
        string newRules = "Updated these!";
        await _spoolRepository.SaveRulesAsync(testSpool.Id, newRules);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task DeleteSpool_SpoolExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
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
        Assert.IsTrue(returnedSpool.Rules.Equals(testSpool.Rules));

        //delete spool
        await _spoolRepository.DeleteSpoolAsync(testSpool.Id);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task DeleteSpool_SpoolNotExist_ShouldPass()
    {
        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //delete spool
        await _spoolRepository.DeleteSpoolAsync(testSpool.Id);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);
    }

    [Test]
    public async Task DeleteSpool_AlsoDeletesThreads_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
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
        Assert.IsTrue(returnedSpool.Rules.Equals(testSpool.Rules));


        // Create Thread
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title",
            Content = "Thread Content",
            OwnerId = testUser.Id,
            SpoolId = testSpool.Id
        };
        // Ensure Thread is not in database
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync(testThread);
        Assert.That(returnedThread, Is.Null);
        // Add Thread to database
        await _threadRepository.InsertThreadAsync(testThread);
        returnedThread = await _threadRepository.GetThreadAsync(testThread);
        Assert.That(returnedThread, Is.Not.Null);

        //delete spool
        await _spoolRepository.DeleteSpoolAsync(testSpool.Id);
        returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        //retrieve thread
        returnedThread = await _threadRepository.GetThreadAsync(testThread);
        Assert.That(returnedThread, Is.Null);
    }

    [Test]
    public async Task GetJoinedSpools_UserExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        //create user2
        User testUser2 = new User()
        {
            Id = "d51c0496-7111-4233-a15f-42aa93c65abf",
            Username = "testUser2",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser2);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
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
        Assert.IsTrue(returnedSpool.Rules.Equals(testSpool.Rules));

        // Create Spool2
        Spool testSpool2 = new Spool()
        {
            Id = "c2ccf78e-b78f-4e9e-8063-8f1ac8ff83c6",
            Name = "Spool2",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool2.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool2);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool2.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool2, Is.Not.Null);
        Assert.IsTrue(returnedSpool2!.Id.Equals(testSpool2.Id));
        Assert.IsTrue(returnedSpool2.Name.Equals(testSpool2.Name));
        Assert.IsTrue(returnedSpool2.OwnerId.Equals(testSpool2.OwnerId));
        Assert.IsTrue(returnedSpool2.Interests.Equals(testSpool2.Interests));
        Assert.IsTrue(returnedSpool2.Rules.Equals(testSpool2.Rules));

        // Create Spool3
        Spool testSpool3 = new Spool()
        {
            Id = "03a62c08-bee4-4e77-823f-3d1d47b494d3",
            Name = "Spool3",
            OwnerId = testUser2.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool3.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool3);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool3.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool2, Is.Not.Null);
        Assert.IsTrue(returnedSpool2!.Id.Equals(testSpool3.Id));
        Assert.IsTrue(returnedSpool2.Name.Equals(testSpool3.Name));
        Assert.IsTrue(returnedSpool2.OwnerId.Equals(testSpool3.OwnerId));
        Assert.IsTrue(returnedSpool2.Interests.Equals(testSpool3.Interests));
        Assert.IsTrue(returnedSpool2.Rules.Equals(testSpool3.Rules));

        // Create Spool4
        Spool testSpool4 = new Spool()
        {
            Id = "1ef832c3-a775-477a-9717-975f32f745a3",
            Name = "Spool4",
            OwnerId = testUser2.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool4.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool4);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool4.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool2, Is.Not.Null);
        Assert.IsTrue(returnedSpool2.Id.Equals(testSpool4.Id));
        Assert.IsTrue(returnedSpool2.Name.Equals(testSpool4.Name));
        Assert.IsTrue(returnedSpool2.OwnerId.Equals(testSpool4.OwnerId));
        Assert.IsTrue(returnedSpool2.Interests.Equals(testSpool4.Interests));
        Assert.IsTrue(returnedSpool2.Rules.Equals(testSpool4.Rules));

        //join the user to the two spools it did not create
        await _userSettingsRepository.JoinUserSettingsAsync(testUser2.Id, testSpool.Name);
        await _userSettingsRepository.JoinUserSettingsAsync(testUser2.Id, testSpool2.Name);

        //get joined
        List<Spool> spoolsJoinedReturn = await _spoolRepository.GetJoinedSpoolsAsync(testUser2.Id);
        Assert.False(spoolsJoinedReturn.IsNullOrEmpty());
        Assert.True(spoolsJoinedReturn.Contains(testSpool));
        Assert.True(spoolsJoinedReturn.Contains(testSpool2));
        Assert.True(spoolsJoinedReturn.Contains(testSpool3));
        Assert.True(spoolsJoinedReturn.Contains(testSpool4));
    }

    [Test]
    public async Task GetJoinedSpools_UserNotExist_ShouldPass()
    {
        //create user
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //get joined
        List<Spool> spoolsJoined = await _spoolRepository.GetJoinedSpoolsAsync(testUser.Id);
        Assert.That(spoolsJoined, Is.Not.Null);
        Assert.True(spoolsJoined.IsNullOrEmpty());
    }

    [Test]
    public async Task GetAllSpools_Exist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
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
        Assert.IsTrue(returnedSpool.Rules.Equals(testSpool.Rules));

        // Create Spool2
        Spool testSpool2 = new Spool()
        {
            Id = "c2ccf78e-b78f-4e9e-8063-8f1ac8ff83c6",
            Name = "Spool2",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool2.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool2);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool2.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool2, Is.Not.Null);
        Assert.IsTrue(returnedSpool2!.Id.Equals(testSpool2.Id));
        Assert.IsTrue(returnedSpool2.Name.Equals(testSpool2.Name));
        Assert.IsTrue(returnedSpool2.OwnerId.Equals(testSpool2.OwnerId));
        Assert.IsTrue(returnedSpool2.Interests.Equals(testSpool2.Interests));
        Assert.IsTrue(returnedSpool2.Rules.Equals(testSpool2.Rules));

        // Create Spool3
        Spool testSpool3 = new Spool()
        {
            Id = "03a62c08-bee4-4e77-823f-3d1d47b494d3",
            Name = "Spool3",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool3.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool3);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool3.Id);

        // Ensure Spool is added correctly
        Assert.That(returnedSpool2, Is.Not.Null);
        Assert.IsTrue(returnedSpool2!.Id.Equals(testSpool3.Id));
        Assert.IsTrue(returnedSpool2.Name.Equals(testSpool3.Name));
        Assert.IsTrue(returnedSpool2.OwnerId.Equals(testSpool3.OwnerId));
        Assert.IsTrue(returnedSpool2.Interests.Equals(testSpool3.Interests));
        Assert.IsTrue(returnedSpool2.Rules.Equals(testSpool3.Rules));

        // Create Spool4
        Spool testSpool4 = new Spool()
        {
            Id = "1ef832c3-a775-477a-9717-975f32f745a3",
            Name = "Spool4",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool4.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Add Spool to database
        await _spoolRepository.InsertSpoolAsync(testSpool4);
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool4.Id);

        //get all spools
        Spool[] allSpools = await _spoolRepository.GetAllSpoolsAsync();
        Assert.False(allSpools.IsNullOrEmpty());
        Assert.True(allSpools.Contains(testSpool));
        Assert.True(allSpools.Contains(testSpool2));
        Assert.True(allSpools.Contains(testSpool3));
        Assert.True(allSpools.Contains(testSpool4));
    }

    [Test]
    public async Task GetAllSpools_NoneExist_ShouldPass()
    {
        //create user
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
        Assert.That(returnedUser, Is.Not.Null);

        // Create Spool
        Spool testSpool = new Spool()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool.Id);
        Assert.That(returnedSpool, Is.Null);

        // Create Spool2
        Spool testSpool2 = new Spool()
        {
            Id = "c2ccf78e-b78f-4e9e-8063-8f1ac8ff83c6",
            Name = "Spool2",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        Spool? returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool2.Id);
        Assert.That(returnedSpool2, Is.Null);

        // Create Spool3
        Spool testSpool3 = new Spool()
        {
            Id = "03a62c08-bee4-4e77-823f-3d1d47b494d3",
            Name = "Spool3",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool3.Id);
        Assert.That(returnedSpool2, Is.Null);
        // Create Spool4
        Spool testSpool4 = new Spool()
        {
            Id = "1ef832c3-a775-477a-9717-975f32f745a3",
            Name = "Spool4",
            OwnerId = testUser.Id,
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { },
            Rules = "First Rules"
        };
        // Ensure Spool is not in database
        returnedSpool2 = await _spoolRepository.GetSpoolAsync(testSpool4.Id);
        Assert.That(returnedSpool2, Is.Null);

        //get all spools
        Spool[] allSpools = await _spoolRepository.GetAllSpoolsAsync();
        Assert.True(allSpools.IsNullOrEmpty());
    }
}