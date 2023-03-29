using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserSessionServiceTests
{
	private UserSessionService _userSessionService;
	private PostgresDbContext _dbContext;

	[SetUp]
	public void Setup()
	{
		_dbContext = CommonUtils.GetDbContext();
		_userSessionService = new UserSessionService(_dbContext);
	}

	[Test]
	public async Task RetrieveUserSession_NotExists_ShouldFail()
	{
		UserSession testUserSession = new UserSession()
		{
			Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
			UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
		};
		UserSession? returnedUserSession = await _userSessionService.GetUserSessionAsync(testUserSession);

		Assert.That(returnedUserSession, Is.Null);
	}

	[Test]
	public async Task RetrieveUserSessionById_NotExists_ShouldFail()
	{
		UserSession? returnedUserSession = await _userSessionService.GetUserSessionAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

		Assert.That(returnedUserSession, Is.Null);
	}

	[Test]
	public async Task RetrieveUserSession_Exists_ShouldPass()
	{
		// Create UserSession
		UserSession testUserSession = new UserSession()
		{
			Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
			UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
		};
		await _dbContext.UserSessions.AddAsync(testUserSession);
		await _dbContext.SaveChangesAsync();

		// Retrieve UserSession
		UserSession? returnedUserSession = await _userSessionService.GetUserSessionAsync(testUserSession);

		Assert.That(returnedUserSession, Is.Not.Null);
		Assert.That(returnedUserSession!.Id, Is.EqualTo(testUserSession.Id));
		Assert.That(returnedUserSession.UserId, Is.EqualTo(testUserSession.UserId));
	}

	[Test]
	public async Task RetrieveUserSessionById_Exists_ShouldPass()
	{
		// Create UserSession
		UserSession testUserSession = new UserSession()
		{
			Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
			UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
		};
		await _dbContext.UserSessions.AddAsync(testUserSession);
		await _dbContext.SaveChangesAsync();

		// Retrieve UserSession
		UserSession? returnedUserSession = await _userSessionService.GetUserSessionAsync(testUserSession.Id);

		Assert.That(returnedUserSession, Is.Not.Null);
		Assert.That(returnedUserSession!.Id, Is.EqualTo(testUserSession.Id));
		Assert.That(returnedUserSession.UserId, Is.EqualTo(testUserSession.UserId));
	}

	[Test]
	public async Task RetrieveUserBySessionId_Expired_ShouldFail()
	{
		// Create UserSession
		UserSession testUserSession = new UserSession()
		{
			Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
			UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
			DateExpires = DateTime.UtcNow.AddDays(-5)
		};
		await _dbContext.UserSessions.AddAsync(testUserSession);
		await _dbContext.SaveChangesAsync();

		// Retrieve UserSession
		UserDTO? returnedUserSession = await _userSessionService.GetUserFromSession(testUserSession.Id);

		Assert.That(returnedUserSession, Is.Null);
	}

	[Test]
	public async Task DeleteUserSession_Exists_ShouldPass()
	{
		// Create UserSession
		UserSession testUserSession = new UserSession()
		{
			Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
			UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
		};
		await _dbContext.UserSessions.AddAsync(testUserSession);
		await _dbContext.SaveChangesAsync();

		// Delete UserSession
		await _userSessionService.DeleteUserSessionAsync(testUserSession.Id);

		// Retrieve UserSession
		UserSession? returnedUserSession = await _userSessionService.GetUserSessionAsync(testUserSession);

		Assert.That(returnedUserSession, Is.Null);
	}
}
