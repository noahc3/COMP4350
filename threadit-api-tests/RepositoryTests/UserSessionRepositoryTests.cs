using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class UserSessionRepositoryTests
{
    private UserSessionRepository _userSessionRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _userSessionRepository = new UserSessionRepository(_dbContext);
    }

    [Test]
    public async Task RetrieveUserSession_NotExists_ShouldFail()
    {
        UserSession testUserSession = new UserSession()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            UserId = "d94ddc51-9031-4e9b-b712-6df32cd75641"
        };
        UserSession? returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);
        
        Assert.That(returnedUserSession, Is.Null);
    }

    [Test]
    public async Task RetrieveUserSessionById_NotExists_ShouldFail()
    {
        UserSession? returnedUserSession = await _userSessionRepository.GetUserSessionAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

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

        // Ensure UserSession is not in database
        UserSession? returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);
        Assert.That(returnedUserSession, Is.Null);

        // Add UserSession to database
        await _userSessionRepository.InsertUserSessionAsync(testUserSession);
        returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);

        // Ensure UserSession is added correctly
        Assert.That(returnedUserSession, Is.Not.Null);
        Assert.IsTrue(returnedUserSession.Id.Equals(testUserSession.Id));
        Assert.IsTrue(returnedUserSession.UserId.Equals(testUserSession.UserId));
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

        // Ensure UserSession is not in database
        UserSession? returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession.Id);
        Assert.That(returnedUserSession, Is.Null);

        // Add UserSession to database
        await _userSessionRepository.InsertUserSessionAsync(testUserSession);
        returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession.Id);

        // Ensure UserSession is added correctly
        Assert.That(returnedUserSession, Is.Not.Null);
        Assert.IsTrue(returnedUserSession.Id.Equals(testUserSession.Id));
        Assert.IsTrue(returnedUserSession.UserId.Equals(testUserSession.UserId));
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

        // Ensure UserSession is not in database
        UserSession? returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);
        Assert.That(returnedUserSession, Is.Null);

        // Add UserSession to database
        await _userSessionRepository.InsertUserSessionAsync(testUserSession);
        returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);

        // Ensure UserSession is added correctly
        Assert.That(returnedUserSession, Is.Not.Null);
        Assert.IsTrue(returnedUserSession.Id.Equals(testUserSession.Id));
        Assert.IsTrue(returnedUserSession.UserId.Equals(testUserSession.UserId));
        
        // Delete UserSession from database
        await _userSessionRepository.DeleteUserSessionAsync(testUserSession.Id);
        returnedUserSession = await _userSessionRepository.GetUserSessionAsync(testUserSession);

        // Ensure UserSession is deleted correctly
        Assert.That(returnedUserSession, Is.Null);
    }
}