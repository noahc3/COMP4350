using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class ThreadRepositoryTests
{
    private ThreadRepository _threadRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _threadRepository = new ThreadRepository(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task RetrieveThread_NotExists_ShouldFail()
    {
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title",
            Content = "Thread Content",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641"
        };
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync(testThread);
        
        Assert.That(returnedThread, Is.Null);
    }

    [Test]
    public async Task RetrieveThreadById_NotExists_ShouldFail()
    {
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");
        
        Assert.That(returnedThread, Is.Null);
    }

    [Test]
    public async Task RetrieveThread_Exists_ShouldPass()
    {
        // Create Thread
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title",
            Content = "Thread Content",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641"
        };

        // Ensure Thread is not in database
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync(testThread);
        Assert.That(returnedThread, Is.Null);

        // Add Thread to database
        await _threadRepository.InsertThreadAsync(testThread);
        returnedThread = await _threadRepository.GetThreadAsync(testThread);

        // Ensure Thread is added correctly
        Assert.That(returnedThread, Is.Not.Null);
        Assert.IsTrue(returnedThread.Id.Equals(testThread.Id));
        Assert.IsTrue(returnedThread.Topic.Equals(testThread.Topic));
        Assert.IsTrue(returnedThread.Title.Equals(testThread.Title));
        Assert.IsTrue(returnedThread.Content.Equals(testThread.Content));
        Assert.IsTrue(returnedThread.OwnerId.Equals(testThread.OwnerId));
        Assert.IsTrue(returnedThread.SpoolId.Equals(testThread.SpoolId));
    }

    [Test]
    public async Task RetrieveThreadById_Exists_ShouldPass()
    {
        // Create Thread
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "bdf89c51-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title",
            Content = "Thread Content",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641"
        };

        // Ensure Thread is not in database
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync(testThread.Id);
        Assert.That(returnedThread, Is.Null);

        // Add Thread to database
        await _threadRepository.InsertThreadAsync(testThread);
        returnedThread = await _threadRepository.GetThreadAsync(testThread.Id);

        // Ensure Thread is added correctly
        Assert.That(returnedThread, Is.Not.Null);
        Assert.IsTrue(returnedThread.Id.Equals(testThread.Id));
        Assert.IsTrue(returnedThread.Topic.Equals(testThread.Topic));
        Assert.IsTrue(returnedThread.Title.Equals(testThread.Title));
        Assert.IsTrue(returnedThread.Content.Equals(testThread.Content));
        Assert.IsTrue(returnedThread.OwnerId.Equals(testThread.OwnerId));
        Assert.IsTrue(returnedThread.SpoolId.Equals(testThread.SpoolId));
    }
}