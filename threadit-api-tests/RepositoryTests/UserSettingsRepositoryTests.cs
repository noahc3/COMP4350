using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserSettingsRepositoryTests
{
    private UserSettingsRepository _userSettingsRepository;
    private PostgresDbContext _dbContext;
    private UserRepository _userRepository;
    private SpoolRepository _spoolRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _userSettingsRepository = new UserSettingsRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
        _spoolRepository = new SpoolRepository(_dbContext);
    }

    [Test]
    public async Task GetUserSettings_NoneExist_ShouldPass()
    {
        //UserSettings
        UserSettings testUserSettings = new UserSettings()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641"
        };

        //get the user settings
        UserSettings? settings = await _userSettingsRepository.GetUserSettingsAsync(testUserSettings);
        Assert.That(settings, Is.Null);
    }

    [Test]
    public async Task GetUserSettingsById_NoneExist_ShouldPass()
    {
        //UserSettings
        UserSettings testUserSettings = new UserSettings()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641"
        };

        //get the user settings
        UserSettings? settings = await _userSettingsRepository.GetUserSettingsAsync(testUserSettings.Id);
        Assert.That(settings, Is.Null);
    }

    [Test]
    public async Task GetUserSettings_Exist_ShouldPass()
    {
        //User
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

        //create a user Settings
        //UserSettings
        UserSettings testUserSettings = new UserSettings()
        {
            Id = testUser.Id
        };

        //get the user settings
        UserSettings? settings = await _userSettingsRepository.GetUserSettingsAsync(testUserSettings);
        Assert.That(settings, Is.Not.Null);
    }

    [Test]
    public async Task GetUserSettingsById_Exist_ShouldPass()
    {
        //User
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

        //get the user settings
        UserSettings? settings = await _userSettingsRepository.GetUserSettingsAsync(testUser.Id);
        Assert.That(settings, Is.Not.Null);
    }

    [Test]
    public async Task RemoveUserSettings_NoneExist_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //create spool
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

        //remove spool for user
        try
        {
            UserSettings returnedSettings = await _userSettingsRepository.RemoveUserSettingsAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task RemoveUserSettings_Exist_ShouldPass()
    {
        //User
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

        //create spool
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


        //remove spool for user
        try
        {
            UserSettings returnedSettings = await _userSettingsRepository.RemoveUserSettingsAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task JoinUserSettings_NoneExist_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //create spool
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


        //join spool for user
        try
        {
            UserSettings returnedSettings = await _userSettingsRepository.JoinUserSettingsAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task JoinUserSettings_UserNotExist_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //create spool
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
        try
        {
            //needs to be in try because adding a spool where the user is not in the database throws an error
            // Add Spool to database
            await _spoolRepository.InsertSpoolAsync(testSpool);
            returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);
            Assert.That(returnedSpool, Is.Not.Null);


            //join spool for user
            UserSettings returnedSettings = await _userSettingsRepository.JoinUserSettingsAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task JoinUserSettings_SpoolNotExist_ShouldPass()
    {
        //User
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

        //create spool
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


        //join spool for user
        try
        {
            UserSettings returnedSettings = await _userSettingsRepository.JoinUserSettingsAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task JoinUserSettings_Exist_ShouldPass()
    {
        //User
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

        //User2
        User testUser2 = new User()
        {
            Id = "9850f4c7-f6cd-413d-a67c-a02571afb4b8",
            Username = "testUser2",
            Email = "testUser2@test.com"
        };
        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser2);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser, Is.Not.Null);

        //create spool
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

        //join spool for user
        UserSettings returnedSettings = await _userSettingsRepository.JoinUserSettingsAsync(testUser.Id, testSpool.Name);
        Assert.That(returnedSettings, Is.Not.Null);

        returnedSettings = await _userSettingsRepository.JoinUserSettingsAsync(testUser2.Id, testSpool.Name);
        Assert.That(returnedSettings, Is.Not.Null);
    }

    [Test]
    public async Task CheckSpoolUser_NoneExist_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //create spool
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


        //check spool for user
        try
        {
            bool returnedValue = await _userSettingsRepository.CheckSpoolUserAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task CheckSpoolUser_UserNotExist_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //create spool
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
        try
        {
            //need this wrapped in try because adding a spool to the DB where the ownerId is not in the users table throws exception
            await _spoolRepository.InsertSpoolAsync(testSpool);
            returnedSpool = await _spoolRepository.GetSpoolAsync(testSpool);


            //check spool for user
            bool returnedValue = await _userSettingsRepository.CheckSpoolUserAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task CheckSpoolUser_SpoolNotExist_ShouldPass()
    {
        //User
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

        //create spool
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


        //check spool for user
        try
        {
            bool returnedValue = await _userSettingsRepository.CheckSpoolUserAsync(testUser.Id, testSpool.Id);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task CheckSpoolUser_Exist_ShouldPass()
    {
        //User
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

        //create spool
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


        //check spool for user
        bool inSpool = await _userSettingsRepository.CheckSpoolUserAsync(testUser.Id, testSpool.Name);
        Assert.True(inSpool);
    }

    [Test]
    public async Task InsertUserSettings_NotExists_ShouldPass()
    {
        //User
        User testUser = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "testUser",
            Email = "testUser@test.com"
        };
        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        //ensure UserSettings not in database
        UserSettings? returnedSettings = await _userSettingsRepository.GetUserSettingsAsync(testUser.Id);
        Assert.That(returnedSettings, Is.Null);

        //add usersettings to db
        UserSettings testUserSettings = new UserSettings()
        {
            Id = testUser.Id
        };
        await _userSettingsRepository.InsertUserSettingsAsync(testUserSettings);

        //get userSettings again
        returnedSettings = await _userSettingsRepository.GetUserSettingsAsync(testUser.Id);
        Assert.That(returnedSettings, Is.Not.Null);
    }

    [Test]
    public async Task InsertUserSettings_Exists_ShouldPass()
    {
        //User
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

        //ensure UserSettings is in database
        UserSettings? returnedSettings = await _userSettingsRepository.GetUserSettingsAsync(testUser.Id);
        Assert.That(returnedSettings, Is.Not.Null);

        //add usersettings to db
        UserSettings testUserSettings = new UserSettings()
        {
            Id = testUser.Id
        };
        try
        {
            await _userSettingsRepository.InsertUserSettingsAsync(testUserSettings);
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public void UserInterestInvalidTests()
    {
        Assert.ThrowsAsync<Exception>(async () => { await _userSettingsRepository.AddUserInterestAsync("invalid", "invalid"); });
        Assert.ThrowsAsync<Exception>(async () => { await _userSettingsRepository.RemoveUserInterestAsync("invalid", "invalid"); });
        Assert.ThrowsAsync<Exception>(async () => { await _userSettingsRepository.BelongInterestAsync("invalid", "invalid"); });
    }
}
