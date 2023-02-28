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

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _userSettingsRepository = new UserSettingsRepository(_dbContext);
    }

    [Test]
    public async Task GetUserSettings_NoneExist_ShouldPass()
    {
    }

    [Test]
    public async Task GetUserSettingsById_NoneExist_ShouldPass()
    {
    }

    [Test]
    public async Task GetUserSettings_Exist_ShouldPass()
    {
    }

    [Test]
    public async Task GetUserSettingsById_Exist_ShouldPass()
    {
    }

    [Test]
    public async Task RemoveUserSettings_NoneExist_ShouldPass()
    {
    }

    [Test]
    public async Task RemoveUserSettings_Exist_ShouldPass()
    {
    }

    [Test]
    public async Task JoinUserSettings_NoneExist_ShouldPass()
    {
    }

    [Test]
    public async Task JoinUserSettings_UserNotExist_ShouldPass()
    {
    }

    [Test]
    public async Task JoinUserSettings_SpoolNotExist_ShouldPass()
    {
    }

    [Test]
    public async Task CheckSpoolUser_NoneExist_ShouldPass()
    {
    }

    [Test]
    public async Task CheckSpoolUser_UserNotExist_ShouldPass()
    {
    }

    [Test]
    public async Task CheckSpoolUser_SpoolNotExist_ShouldPass()
    {
    }
}