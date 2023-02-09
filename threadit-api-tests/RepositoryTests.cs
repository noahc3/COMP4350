using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditTests.Repositories;

public class Tests
{
    private UserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        _userRepository = new UserRepository(new PostgresDbContext());
    }

    [Test]
    public async Task UserRepository_RetrieveUser_NotExists_ShouldFail()
    {
        UserDTO? returnedUser = await _userRepository.GetUserAsync("00000000-0000-0000-0000-000000000000");

        Assert.That(returnedUser, Is.Null);
    }

    [Test]
    public async Task UserRepository_RetrieveUser_Exists_ShouldPass()
    {
        User testUser = new User() {
            Username = "testUsername",
            Email = "testUserEmail@test.com"
        };

        await _userRepository.InsertUserAsync(testUser);

        UserDTO? returnedUser = await _userRepository.GetUserAsync(testUser.Id.ToString());

        Assert.That(returnedUser, Is.Not.Null);
        Assert.IsTrue(returnedUser.Id.Equals(testUser.Id));
        Assert.IsTrue(returnedUser.Email.Equals(testUser.Email));
        Assert.IsTrue(returnedUser.Username.Equals(testUser.Username));
        Assert.IsTrue(returnedUser.DateCreated.ToString().Equals(testUser.DateCreated.ToString()));
    }
}