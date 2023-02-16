using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class ThreadServiceTests
{
    private ThreadService _threadService;
    private SpoolRepository _spoolRepository;
    private UserRepository _userRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _threadService = new ThreadService(_dbContext);
        _spoolRepository = new SpoolRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task RetrieveThreadsBySpool_NoneExists_ShouldFail()
    {
        Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadsBySpoolAsync("qr5t9c51-9031-4e9b-b712-6df32cd75641"));
    }

    [Test]
    public async Task RetrieveThreadsBySpool_Exists_ShouldPass()
    {
        // Create User
        var user = new ThreaditAPI.Models.User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "TestUser",
            Email = "test@test.com",
            PasswordHash = "password",
        };

        await _userRepository.InsertUserAsync(user);

        // Create Spools
        var spools = new List<ThreaditAPI.Models.Spool>() {
            new ThreaditAPI.Models.Spool() {
                Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                Name = "Spool1",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            },
            new ThreaditAPI.Models.Spool() {
                Id = "adadada-9031-4e9b-b712-6df32cd75641",
                Name = "Spool2",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            }
        };

        foreach(var spool in spools)
        {
            await _spoolRepository.InsertSpoolAsync(spool);
        }

        // Create Threads
        var threads = new List<ThreaditAPI.Models.Thread>() {
            new ThreaditAPI.Models.Thread()
            {
                Id = "643634634-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 1",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime()
            },
            new ThreaditAPI.Models.Thread()
            {
                Id = "12321314-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 2",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "adadada-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 01:00:00").ToUniversalTime()
            },
            new ThreaditAPI.Models.Thread()
            {
                Id = "2563636-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 3",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 02:00:00").ToUniversalTime()
            }
        };

        // Ensure Thread is not in database
        foreach(var thread in threads)
        {
            Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread));
        }

        // Ensure query by spool returns no items
        var returnedThreads = await _threadService.GetThreadsBySpoolAsync("Spool1");
        Assert.That(returnedThreads, Is.Empty);

        // Add Thread to database
        foreach(var thread in threads)
        {
            await _threadService.InsertThreadAsync(thread);
        }

        // Ensure Thread is added correctly
        returnedThreads = await _threadService.GetThreadsBySpoolAsync("Spool1");
        Assert.That(returnedThreads, Is.Not.Null);
        Assert.AreEqual(2, returnedThreads.Count);
        Assert.AreEqual("Thread Title 3", returnedThreads[0].Title);
        Assert.AreEqual("Spool1", returnedThreads[0].SpoolName);
        Assert.AreEqual("TestUser", returnedThreads[0].AuthorName);
        Assert.AreEqual("Thread Title 1", returnedThreads[1].Title);
        Assert.AreEqual("Spool1", returnedThreads[1].SpoolName);
        Assert.AreEqual("TestUser", returnedThreads[1].AuthorName);
    }

    [Test]
    public async Task RetrieveAllThreads_NoneExists_ShouldFail()
    {
        var threads = await _threadService.GetAllThreadsAsync();

        Assert.That(threads, Is.Empty);
    }

    [Test]
    public async Task RetrieveAllThreads_Exists_ShouldPass()
    {
        // Create User
        var user = new ThreaditAPI.Models.User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "TestUser",
            Email = "test@test.com",
            PasswordHash = "password",
        };

        await _userRepository.InsertUserAsync(user);

        // Create Spools
        var spools = new List<ThreaditAPI.Models.Spool>() {
            new ThreaditAPI.Models.Spool() {
                Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                Name = "Spool1",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            },
            new ThreaditAPI.Models.Spool() {
                Id = "adadada-9031-4e9b-b712-6df32cd75641",
                Name = "Spool2",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                Interests = new List<string>() { "Interest1", "Interest2" },
                Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
            }
        };

        foreach(var spool in spools)
        {
            await _spoolRepository.InsertSpoolAsync(spool);
        }

        // Create Threads
        var threads = new List<ThreaditAPI.Models.Thread>() {
            new ThreaditAPI.Models.Thread()
            {
                Id = "643634634-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 1",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime()
            },
            new ThreaditAPI.Models.Thread()
            {
                Id = "12321314-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 2",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "adadada-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 01:00:00").ToUniversalTime()
            },
            new ThreaditAPI.Models.Thread()
            {
                Id = "2563636-9031-4e9b-b712-6df32cd75641",
                Topic = "Thread Topic",
                Title = "Thread Title 3",
                Content = "Thread Content",
                OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
                SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
                DateCreated = DateTime.Parse("2021-01-01 02:00:00").ToUniversalTime()
            }
        };

        // Ensure Thread is not in database
        foreach(var thread in threads)
        {
            Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread));
        }

        // Ensure query by spool returns no items
        var returnedThreads = await _threadService.GetAllThreadsAsync();
        Assert.That(returnedThreads, Is.Empty);

        // Add Thread to database
        foreach(var thread in threads)
        {
            await _threadService.InsertThreadAsync(thread);
        }

        // Ensure Thread is added correctly
        returnedThreads = await _threadService.GetAllThreadsAsync();
        Assert.That(returnedThreads, Is.Not.Null);
        Assert.That(returnedThreads.Count, Is.EqualTo(3));
        Assert.AreEqual("Thread Title 3", returnedThreads[0].Title);
        Assert.AreEqual("Spool1", returnedThreads[0].SpoolName);
        Assert.AreEqual("TestUser", returnedThreads[0].AuthorName);
        Assert.AreEqual("Thread Title 2", returnedThreads[1].Title);
        Assert.AreEqual("Spool2", returnedThreads[1].SpoolName);
        Assert.AreEqual("TestUser", returnedThreads[1].AuthorName);
        Assert.AreEqual("Thread Title 1", returnedThreads[2].Title);
        Assert.AreEqual("Spool1", returnedThreads[2].SpoolName);
        Assert.AreEqual("TestUser", returnedThreads[2].AuthorName);
    }
}