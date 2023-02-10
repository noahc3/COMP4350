using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

//ALL TESTS THAT NEED TO ADD A USER SHOULD USE THE UserId "bdf89c51-9031-4e9b-b712-6df32cd75641" OR "372a4322-21cd-46f7-9ce9-253d6ff62e13"
//AS WELL AS "doesNotExistUser@test.com" OR "doesNotExistUser@test.net"
//AND "doesNotExistUser" OR "fakeUser"

public class RepositoryTests
{
    private UserRepository _userRepository;
    private PostgresDbContext _dbContext = new PostgresDbContext();

    [SetUp]
    public void Setup()
    {
        _userRepository = new UserRepository(_dbContext);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task UserRepository_RetrieveUser_NotExists_ShouldFail()
    {
        UserDTO? returnedUser = await _userRepository.GetUserAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

        Assert.That(returnedUser, Is.Null);

        //also check with giving it a user entity which has not been added
        UserDTO initialUser = new User()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };
        returnedUser = await _userRepository.GetUserAsync(initialUser);
        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task UserRepository_RetrieveUser_Exists_ShouldPass()
    {
        //create user
        User testUser = new User()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        //make sure the user was not already in the database
        UserDTO? returnedUser = await _userRepository.GetUserAsync(testUser);
        Assert.That(returnedUser, Is.Null);

        //now add to the database
        await _userRepository.InsertUserAsync(testUser);

        //now get the user from the database
        returnedUser = await _userRepository.GetUserAsync(testUser.Id.ToString());

        //checkit
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));

        //cleanup
        //await _userRepository.DeleteUserAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");
    }
}