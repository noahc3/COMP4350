using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;
using ThreaditAPI.Repositories;

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

    [Test]
    public async Task UpdateThread_Exists_ShouldPass()
    {
        // Create Thread
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "7b55bc38-c3d7-41d2-8f19-c6e6e9bcd50d",
            Topic = "Test Thread Topic",
            Title = "Test Thread Title",
            Content = "Test Thread Content",
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

        //Create updated Thread
        ThreaditAPI.Models.Thread updatedTestThread = new ThreaditAPI.Models.Thread()
        {
            Id = "7b55bc38-c3d7-41d2-8f19-c6e6e9bcd50d",
            Topic = "Updated test topic",
            Title = "Updated test title",
            Content = "Updated test content",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641"
        };

        //update thread in the database
        await _threadRepository.UpdateThreadAsync(updatedTestThread);

        //get the thread again
        returnedThread = await _threadRepository.GetThreadAsync(updatedTestThread.Id);

        //make sure thread has been updated properly
        Assert.That(returnedThread, Is.Not.Null);
        Assert.IsTrue(returnedThread.Id.Equals(updatedTestThread.Id));
        Assert.IsTrue(returnedThread.Topic.Equals(updatedTestThread.Topic));
        Assert.IsTrue(returnedThread.Title.Equals(updatedTestThread.Title));
        Assert.IsTrue(returnedThread.Content.Equals(updatedTestThread.Content));
        Assert.IsTrue(returnedThread.OwnerId.Equals(updatedTestThread.OwnerId));
        Assert.IsTrue(returnedThread.SpoolId.Equals(updatedTestThread.SpoolId));
    }

    [Test]
    public async Task UpdateThread_NotExists_ShouldPass()
    {
        // Create Thread
        ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
        {
            Id = "7b55bc38-c3d7-41d2-8f19-c6e6e9bcd50d",
            Topic = "Test Thread Topic",
            Title = "Test Thread Title",
            Content = "Test Thread Content",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641"
        };

        // Ensure Thread is not in database
        ThreaditAPI.Models.Thread? returnedThread = await _threadRepository.GetThreadAsync(testThread.Id);
        Assert.That(returnedThread, Is.Null);

        //update thread
        await _threadRepository.UpdateThreadAsync(testThread);

        //get the thread again
        returnedThread = await _threadRepository.GetThreadAsync(testThread.Id);

        //make sure it was not in the database
        Assert.That(returnedThread, Is.Null);
    }
}