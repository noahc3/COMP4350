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
    private string longText = "luctus venenatis lectus magna fringilla urna porttitor rhoncus dolor purus non enim praesent elementum facilisis leo vel fringilla est ullamcorper eget nulla facilisi etiam dignissim diam quis enim lobortis scelerisque fermentum dui faucibus in ornare quam viverra orci sagittis eu volutpat odio facilisis mauris sit amet massa vitae tortor condimentum lacinia quis vel eros donec ac odio tempor orci dapibus ultrices in iaculis nunc sed augue lacus viverra vitae congue eu consequat ac felis donec et odio pellentesque diam volutpat commodo sed egestas egestas fringilla phasellus faucibus scelerisque eleifend donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit dignissim sodales ut eu sem integer vitae justo eget magna fermentum iaculis eu non diam phasellus vestibulum lorem sed risus ultricies tristique nulla aliquet enim tortor at auctor urna nunc id cursus metus aliquam eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus semper eget duis at tellus at urna condimentum mattis pellentesque id nibh tortor id aliquet lectus proin nibh nisl condimentum id venenatis a condimentum vitae sapien pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas sed tempus urna et pharetra pharetra massa massa ultricies mi quis hendrerit dolor magna eget est lorem ipsum dolor sit amet consectetur adipiscing elit pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas integer eget aliquet nibh praesent tristique magna sit amet purus gravida quis blandit turpis cursus in hac habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat mauris nunc congue nisi vitae suscipit tellus mauris a diam maecenas sed enim ut sem viverra aliquet eget sit amet tellus cras adipiscing enim eu turpis egestas pretium aenean pharetra magna ac placerat vestibulum lectus mauris ultrices eros in cursus turpis massa tincidunt dui ut ornare lectus sit amet est placerat in egestas erat imperdiet sed euismod nisi porta lorem mollis aliquam ut porttitor leo a diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper sit amet risus nullam eget felis eget nunc lobortis mattis aliquam faucibus purus inplacerat in egestas erat imperdiet sed euismod nisi porta lorem mollis aliquam ut porttitor leo a diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper sit amet risus nullam eget felis eget nunc lobortis mattis aliquam faucibus purus in";
    private string longTitle = "luctus venenatis lectus magna fringilla urna porttitor rhoncus dolor purus non enim praesent elementum facilisis leo vel fringilla est ullamcorper eget nulla facilisi etiam dignissim diam quis enim lobortis scelerisque fermentum dui faucibus in ornare quam viverra orci sagittis eu volutpat odio facilisis mauris sit amet massa ";

    [SetUp]
    public void Setup()
    {
        _dbContext = CommonUtils.GetDbContext();
        _threadService = new ThreadService(_dbContext);
        _spoolRepository = new SpoolRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
    }

    [Test]
    public async Task InsertThread_NotValid_ShouldFail()
    {
        // Create User
        var user = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Username = "TestUser",
            Email = "test@test.com",
            PasswordHash = "password",
        };

        var spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool1",
            OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
            Interests = new List<string>() { "Interest1", "Interest2" },
            Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
        };

        //empty title
        var thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "",
            Content = "Thread Content",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.InsertThreadAsync(thread));

        //title over 256 characters
        thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = longTitle,
            Content = "Thread Content",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.InsertThreadAsync(thread));

        //content over 2048 characters
        thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title 1",
            Content = longText,
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.InsertThreadAsync(thread));

        //type is not correct type
        thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title 1",
            Content = "Thread Content",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = "not valid"
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.InsertThreadAsync(thread));

        //ID already in use
        thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic",
            Title = "Thread Title 1",
            Content = "Thread Content",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };

        // Ensure Thread is not in database
        Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread.Id));

        // Add Thread to database
        await _threadService.InsertThreadAsync(thread);
        var returnedThread = await _threadService.GetThreadAsync(thread.Id);

        // Ensure Thread is added correctly
        Assert.That(returnedThread, Is.Not.Null);
        Assert.IsTrue(returnedThread!.Id.Equals(thread.Id));
        Assert.IsTrue(returnedThread.Topic.Equals(thread.Topic));
        Assert.IsTrue(returnedThread.Title.Equals(thread.Title));
        Assert.IsTrue(returnedThread.Content.Equals(thread.Content));
        Assert.IsTrue(returnedThread.OwnerId.Equals(thread.OwnerId));
        Assert.IsTrue(returnedThread.SpoolId.Equals(thread.SpoolId));

        thread = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75641",
            Topic = "Thread Topic1",
            Title = "Thread Title 11",
            Content = "Thread Content1",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.InsertThreadAsync(thread));

        //update content too long
        var thread2 = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75642",
            Topic = "Thread Topic",
            Title = "Thread Title 1",
            Content = "Thread Content",
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };

        // Ensure Thread is not in database
        Assert.ThrowsAsync<Exception>(async () => await _threadService.GetThreadAsync(thread2.Id));

        // Add Thread to database
        await _threadService.InsertThreadAsync(thread2);
        returnedThread = await _threadService.GetThreadAsync(thread2.Id);

        // Ensure Thread is added correctly
        Assert.That(returnedThread, Is.Not.Null);
        Assert.IsTrue(returnedThread!.Id.Equals(thread2.Id));
        Assert.IsTrue(returnedThread.Topic.Equals(thread2.Topic));
        Assert.IsTrue(returnedThread.Title.Equals(thread2.Title));
        Assert.IsTrue(returnedThread.Content.Equals(thread2.Content));
        Assert.IsTrue(returnedThread.OwnerId.Equals(thread2.OwnerId));
        Assert.IsTrue(returnedThread.SpoolId.Equals(thread2.SpoolId));

        thread2 = new ThreaditAPI.Models.Thread()
        {
            Id = "643634634-9031-4e9b-b712-6df32cd75642",
            Topic = "Thread Topic1",
            Title = "Thread Title 11",
            Content = longText,
            OwnerId = user.Id,
            SpoolId = spool.Id,
            DateCreated = DateTime.Parse("2021-01-01 00:00:00").ToUniversalTime(),
            ThreadType = ThreaditAPI.Constants.ThreadTypes.TEXT
        };
        Assert.ThrowsAsync<Exception>(async () => await _threadService.UpdateThreadAsync(thread2));
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
