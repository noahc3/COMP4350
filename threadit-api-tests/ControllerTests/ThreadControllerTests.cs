using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens;

namespace ThreaditTests.Controllers;
public class ThreadControllerTests
{
    private Spool _spool;
    private ThreaditAPI.Models.Thread _thread;
    private UserDTO _user1;
    private UserDTO _user2;
    private HttpClient _client1;
    private HttpClient _client2;

    [SetUp]
    public void Setup()
    {
        UserDTO _user1Temp;
        UserDTO _user2Temp;
        HttpClient _client1Temp;
        HttpClient _client2Temp;

        (_client1Temp, _user1Temp, _) = Utils.CreateAndAuthenticateUser();
        (_client2Temp, _user2Temp, _) = Utils.CreateAndAuthenticateUser();

        _user1 = _user1Temp;
        _client1 = _client1Temp;
        _user2 = _user1Temp;
        _client2 = _client1Temp;

        _spool = Utils.CreateSpool(_client1, _user1.Id);
        _thread = Utils.CreateThread(_client1, _user1.Id, _spool.Id, title: Utils.GetCleanUUIDString(), content: Utils.GetCleanUUIDString());
    }

    [Test]
    public void CreateUpdateDeleteThreadTest()
    {
        //get the endpoint
        var endpoint = String.Format(Endpoints.V1_THREAD_CREATE);

        //create the thread
        var result = _client1.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(_thread)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        var thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.That(thread1, Is.Not.Null);
        Assert.IsTrue(thread1.OwnerId.Equals(_user1.Id));
        Assert.IsTrue(thread1.SpoolId.Equals(_spool.Id));
        Assert.IsTrue(thread1.Content.Equals(_thread.Content));
        Assert.IsTrue(thread1.Title.Equals(_thread.Title));

        //get the thread
        endpoint = String.Format(Endpoints.V1_THREAD_GET, _thread.Id);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.That(thread1, Is.Not.Null);
        Assert.That(thread1.Id.Equals(_thread.Id));


        //update the thread
        var content2 = Utils.GetCleanUUIDString();
        var title2 = Utils.GetCleanUUIDString();
        endpoint = String.Format(Endpoints.V1_THREAD_EDIT);
        ThreaditAPI.Models.Thread _thread2 = Utils.CreateThread(_client1, _user1.Id, _spool.Id, title: title2, content: content2);
        result = _client1.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(_thread2)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.That(thread1, Is.Not.Null);
        Assert.IsTrue(thread1.OwnerId.Equals(_user1.Id));
        Assert.IsTrue(thread1.SpoolId.Equals(_spool.Id));
        Assert.IsTrue(thread1.Content.Equals(content2));
        Assert.IsTrue(thread1.Title.Equals(title2));

        //delete the thread
        endpoint = String.Format(Endpoints.V1_THREAD_DELETE, _thread.Id);
        result = _client1.DeleteAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);

        //get the thread to ensure its been deleted
        endpoint = String.Format(Endpoints.V1_THREAD_GET, _thread.Id);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void GetAllThreadsTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL);

        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<ThreaditAPI.Models.Thread[]>(result);
        Assert.That(!threads.IsNullOrEmpty());
        var thread = threads!.First();
        Assert.That(thread.Id.Equals(_thread.Id));
        Assert.That(thread.OwnerId.Equals(_thread.OwnerId));
        Assert.That(thread.SpoolId.Equals(_thread.SpoolId));
        Assert.That(thread.Title.Equals(_thread.Title));
        Assert.That(thread.Topic.Equals(_thread.Topic));
    }

    [Test]
    public void DeleteThreadsInvalidTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_DELETE, _thread.Id);
        try
        {
            var result = _client2.DeleteAsync(endpoint).Result;
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }

        endpoint = String.Format(Endpoints.V1_THREAD_DELETE, "");
        try
        {
            var result = _client2.DeleteAsync(endpoint).Result;
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public void EditBadThreadTests()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_EDIT);
        ThreaditAPI.Models.Thread editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = "",
            OwnerId = _thread.OwnerId,
            SpoolId = _spool.Id
        };

        try
        {
            var result = _client1.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }

        editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = _thread.OwnerId,
            SpoolId = _spool.Id
        };

        var failedResult = _client1.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
        Assert.IsFalse(failedResult.IsSuccessStatusCode);

        editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = _thread.Id,
            OwnerId = _thread.OwnerId,
            SpoolId = _spool.Id
        };

        try
        {
            var result = _client1.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }

        //edit while not the owner
        editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = _thread.Id,
            OwnerId = _user1.Id,
            SpoolId = _spool.Id,
            Content = "new content"
        };

        var nonExceptionResult = _client2.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
        Assert.IsFalse(nonExceptionResult.IsSuccessStatusCode);
    }
}