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
		_dbContext = CommonUtils.GetDbContext();
		_threadService = new ThreadService(_dbContext);
		_spoolRepository = new SpoolRepository(_dbContext);
		_userRepository = new UserRepository(_dbContext);
	}

	[Test]
	public void RetrieveThreadsBySpool_NoneExists_ShouldFail()
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

		foreach (var spool in spools)
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
				DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
				ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			},
			new ThreaditAPI.Models.Thread()
			{
				Id = "12321314-9031-4e9b-b712-6df32cd75641",
				Topic = "Thread Topic",
				Title = "Thread Title 2",
				Content = "Thread Content",
				OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
				SpoolId = "adadada-9031-4e9b-b712-6df32cd75641",
				DateCreated = DateTime.Parse("2021-01-01 01:00:00").ToUniversalTime(),
				ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			},
			new ThreaditAPI.Models.Thread()
			{
				Id = "2563636-9031-4e9b-b712-6df32cd75641",
				Topic = "Thread Topic",
				Title = "Thread Title 3",
				Content = "Thread Content",
				OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
				SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
				DateCreated = DateTime.Parse("2021-01-01 02:00:00").ToUniversalTime(),
				ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			}
		};

		// Ensure Thread is not in database
		foreach (var thread in threads)
		{
			Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread.Id));
		}

		// Ensure query by spool returns no items
		var returnedThreads = await _threadService.GetThreadsBySpoolAsync("Spool1");
		Assert.That(returnedThreads, Is.Empty);

		// Add Thread to database
		foreach (var thread in threads)
		{
			await _threadService.InsertThreadAsync(thread);
		}

		// Ensure Thread is added correctly
		returnedThreads = await _threadService.GetThreadsBySpoolAsync("Spool1");
		Assert.That(returnedThreads, Is.Not.Null);
		Assert.That(returnedThreads.Count, Is.EqualTo(2));
		Assert.That(returnedThreads[0].Title, Is.EqualTo("Thread Title 3"));
		Assert.That(returnedThreads[0].SpoolName, Is.EqualTo("Spool1"));
		Assert.That(returnedThreads[0].AuthorName, Is.EqualTo("TestUser"));
		Assert.That(returnedThreads[1].Title, Is.EqualTo("Thread Title 1"));
		Assert.That(returnedThreads[1].SpoolName, Is.EqualTo("Spool1"));
		Assert.That(returnedThreads[1].AuthorName, Is.EqualTo("TestUser"));
	}

	[Test]
	public async Task RetrieveAllThreads_NoneExists_ShouldFail()
	{
		var threads = await _threadService.GetThreadsAsync();

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

		foreach (var spool in spools)
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
				DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
			ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			},
			new ThreaditAPI.Models.Thread()
			{
				Id = "12321314-9031-4e9b-b712-6df32cd75641",
				Topic = "Thread Topic",
				Title = "Thread Title 2",
				Content = "Thread Content",
				OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
				SpoolId = "adadada-9031-4e9b-b712-6df32cd75641",
				DateCreated = DateTime.Parse("2021-01-01 01:00:00").ToUniversalTime(),
			ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			},
			new ThreaditAPI.Models.Thread()
			{
				Id = "2563636-9031-4e9b-b712-6df32cd75641",
				Topic = "Thread Topic",
				Title = "Thread Title 3",
				Content = "Thread Content",
				OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
				SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
				DateCreated = DateTime.Parse("2021-01-01 02:00:00").ToUniversalTime(),
			ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
			}
		};

		// Ensure Thread is not in database
		foreach (var thread in threads)
		{
			Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread.Id));
		}

		// Ensure query by spool returns no items
		var returnedThreads = await _threadService.GetThreadsAsync();
		Assert.That(returnedThreads, Is.Empty);

		// Add Thread to database
		foreach (var thread in threads)
		{
			await _threadService.InsertThreadAsync(thread);
		}

		// Ensure Thread is added correctly
		returnedThreads = await _threadService.GetThreadsAsync();
		Assert.That(returnedThreads, Is.Not.Null);
		Assert.That(returnedThreads.Count, Is.EqualTo(3));
		Assert.That(returnedThreads[0].Title, Is.EqualTo("Thread Title 3"));
		Assert.That(returnedThreads[0].SpoolName, Is.EqualTo("Spool1"));
		Assert.That(returnedThreads[0].AuthorName, Is.EqualTo("TestUser"));
		Assert.That(returnedThreads[1].Title, Is.EqualTo("Thread Title 2"));
		Assert.That(returnedThreads[1].SpoolName, Is.EqualTo("Spool2"));
		Assert.That(returnedThreads[1].AuthorName, Is.EqualTo("TestUser"));
		Assert.That(returnedThreads[2].Title, Is.EqualTo("Thread Title 1"));
		Assert.That(returnedThreads[2].SpoolName, Is.EqualTo("Spool1"));
		Assert.That(returnedThreads[2].AuthorName, Is.EqualTo("TestUser"));
	}

	[Test]
	public async Task DeleteThread_Exists_ShouldPass()
	{
		// Create Thread
		ThreaditAPI.Models.Thread testThread = new ThreaditAPI.Models.Thread()
		{
			Id = "7b55bc38-c3d7-41d2-8f19-c6e6e9bcd50d",
			Topic = "Test Thread Topic",
			Title = "Test Thread Title",
			Content = "Test Thread Content",
			OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
			SpoolId = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
			ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
		};

		// Ensure Thread is not in database
		Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(testThread.Id));

		// Add Thread to database
		await _threadService.InsertThreadAsync(testThread);
		var returnedThread = await _threadService.GetThreadAsync(testThread.Id);

		// Ensure Thread is added correctly
		Assert.That(returnedThread, Is.Not.Null);
		Assert.IsTrue(returnedThread!.Id.Equals(testThread.Id));
		Assert.IsTrue(returnedThread.Topic.Equals(testThread.Topic));
		Assert.IsTrue(returnedThread.Title.Equals(testThread.Title));
		Assert.IsTrue(returnedThread.Content.Equals(testThread.Content));
		Assert.IsTrue(returnedThread.OwnerId.Equals(testThread.OwnerId));
		Assert.IsTrue(returnedThread.SpoolId.Equals(testThread.SpoolId));

		//delete thread
		await _threadService.DeleteThreadAsync(testThread.Id, "d94ddc51-9031-4e9b-b712-6df32cd75641");

		//get the thread again
		Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(testThread.Id));
	}
}
