using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserServiceTests
{
    private UserService _userService;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _userService = new UserService(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task UserService_RetrieveUser_NotExists_ShouldFail()
    {
        //check with giving it a string for the ID
        UserDTO? returnedUser = await _userService.GetUserAsync("00000000-0000-0000-0000-000000000000");
        Assert.That(returnedUser, Is.Null);

        //also check with giving it a user entity which has not been added
        UserDTO initialUser = new User() {
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };
        returnedUser = await _userService.GetUserAsync(initialUser);
        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task UserService_RetrieveUser_Exists_ShouldPass()
    {
        //add to the database
        User createdUser = await _userService.CreateUserAsync("doesNotExistUser", "doesNotExistUser@test.com", "fakePassword");

        //now get the user from the database
        UserDTO? returnedUser = await _userService.GetUserAsync(createdUser.Id.ToString());

        //check it
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(createdUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(createdUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(createdUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(createdUser.DateCreated.ToString()));
    }
}