using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserRepositoryTests
{
    private UserRepository _userRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _userRepository = new UserRepository(_dbContext);
    }

    [Test]
    public async Task RetrieveUser_NotExists_ShouldFail()
    {
        UserDTO initialUser = new User()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };
        UserDTO? returnedUser = await _userRepository.GetUserAsync(initialUser);

        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task RetrieveUserById_NotExists_ShouldFail()
    {
        UserDTO? returnedUser = await _userRepository.GetUserAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task RetrieveUserByLoginIdentifier_NotExists_ShouldFail()
    {
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync("doesNotExistUser");

        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task RetrieveUser_Exists_ShouldPass()
    {
        // Create User
        User testUser = new User()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserAsync(testUser);
        Assert.That(returnedUser, Is.Null);

        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserAsync(testUser);

        // Ensure User is added correctly
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));
    }

    [Test]
    public async Task RetrieveUserById_Exists_ShouldPass()
    {
        // Create User
        User testUser = new User()
        {
            Id = "89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserAsync(testUser.Id);
        Assert.That(returnedUser, Is.Null);

        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserAsync(testUser.Id);

        // Ensure User is added correctly
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));
    }

    [Test]
    public async Task RetrieveUserByLoginIdentifier_Exists_ShouldPass()
    {
        // Create User
        User testUser = new User()
        {
            Id = "89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Ensure User is added correctly
        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));
    }

    [Test]
    public async Task DeleteUser_Exists_ShouldPass()
    {// Create User
        User testUser = new User()
        {
            Id = "89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        // Add User to database
        await _userRepository.InsertUserAsync(testUser);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

        // Ensure User is deleted
        returnedUser = await _userRepository.DeleteUserAsync(testUser.Id);
        Assert.That(returnedUser, Is.Not.Null);
    }

    [Test]
    public async Task DeleteUser_NotExists_ShouldPass()
    {// Create User
        User testUser = new User()
        {
            Id = "89c51-9031-4e9b-b712-6df32cd75641",
            Username = "doesNotExistUser",
            Email = "doesNotExistUser@test.com"
        };

        // Ensure User is not in database
        UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
        Assert.That(returnedUser, Is.Null);

        // Ensure User is not deleted
        returnedUser = await _userRepository.DeleteUserAsync(testUser.Id);
        Assert.That(returnedUser, Is.Null);

    }
}
