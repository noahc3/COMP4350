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
        _dbContext = CommonUtils.GetDbContext();
        _userService = new UserService(_dbContext);
    }

    [Test]
    public async Task UserService_RetrieveUser_NotExists_ShouldFail()
    {
        try
        {
            //check with giving it a string for the ID
            UserDTO? returnedUser = await _userService.GetUserAsync("00000000-0000-0000-0000-000000000000");
            Assert.Fail();
        }
        catch (Exception)
        {
            
        }

        //also check with giving it a user entity which has not been added
        try
        {
            UserDTO initialUser = new User()
            {
                Username = "doesNotExistUser",
                Email = "doesNotExistUser@test.com"
            };
            UserDTO? returnedUser2 = await _userService.GetUserAsync(initialUser.Id);
            Assert.Fail();
        }
        catch(Exception)
        {
            Assert.Pass();
        }
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

    [Test]
    public async Task DeleteUser_Exists_ShouldPass()
    {
        //add to the database
        User createdUser = await _userService.CreateUserAsync("doesNotExistUser", "doesNotExistUser@test.com", "fakePassword");

        //now get the user from the database
        UserDTO? returnedUser = await _userService.GetUserAsync(createdUser.Id.ToString());

        //check it
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(createdUser.Id));

        //now delete the user
        UserDTO? deletedUser = await _userService.DeleteUserAsync(createdUser.Id.ToString());

        //check it
        Assert.That(deletedUser, Is.Not.Null);
        Assert.IsTrue(deletedUser.Id.Equals(createdUser.Id));
        
        //now try to get the user again
        try
        {
            UserDTO? returnedUser2 = await _userService.GetUserAsync(createdUser.Id.ToString());
            Assert.Fail();
        }
        catch (Exception)
        {
            Assert.Pass();
        }
    }

    [Test]
    public void DeleteUser_NotExists_ShouldPass()
    {
        Assert.ThrowsAsync<Exception>(async () => {
            await _userService.DeleteUserAsync("invalid123");
        });
    }
}