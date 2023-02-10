using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

//ALL TESTS THAT NEED TO ADD A USER SHOULD USE THE UserId "bdf89c51-9031-4e9b-b712-6df32cd75641" OR "372a4322-21cd-46f7-9ce9-253d6ff62e13"

public class ServiceTests
{
    private UserService _userService;
    private PostgresDbContext _dbContext = new PostgresDbContext();

    [SetUp]
    public void Setup()
    {
        _userService = new UserService(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        try
        {
            string sqlQuery = "DELETE FROM Users WHERE Id='bdf89c51-9031-4e9b-b712-6df32cd75641';";
            _dbContext.Database.ExecuteSqlRaw(sqlQuery);

            sqlQuery = "DELETE FROM Users WHERE Id='372a4322-21cd-46f7-9ce9-253d6ff62e13';";
            _dbContext.Database.ExecuteSqlRaw(sqlQuery);
        }
        catch (Exception e)
        {
            //its fine, the previous test probably failed before adding the user to the database.
            try
            {
                string sqlQuery = "DELETE FROM Users WHERE Id='372a4322-21cd-46f7-9ce9-253d6ff62e13';";
                _dbContext.Database.ExecuteSqlRaw(sqlQuery);
            }
            catch (Exception ex)
            {
                //still fine.
            }
        }
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
        User testUser = new User() {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };
        //make sure the user was not already in the database
        UserDTO? returnedUser = await _userService.GetUserAsync(testUser);
        Assert.That(returnedUser, Is.Null);

        //now add to the database
        await _userService.CreateUserAsync(testUser.Username, testUser.Email, "fakePassword");

        returnedUser = await _userService.GetUserAsync(testUser.Id.ToString());

        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));

        //cleanup
        string sqlQuery = "DELETE FROM Users WHERE Id='bdf89c51-9031-4e9b-b712-6df32cd75641';";
        _dbContext.Database.ExecuteSqlRaw(sqlQuery);
    }
}