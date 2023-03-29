using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserSettingsServiceTests
{
	private UserSettingsService _userSettingsService;
	private PostgresDbContext _dbContext;

	[SetUp]
	public void Setup()
	{
		_dbContext = CommonUtils.GetDbContext();
		_userSettingsService = new UserSettingsService(_dbContext);
	}

	[Test]
	public void UserSettingsInvalidTest()
	{
		Assert.ThrowsAsync<Exception>(async () => { await _userSettingsService.GetUserSettingsAsync("00000000-0000-0000-0000-000000000000"); });
	}
}
