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
    private UserDTO _user3;
    private HttpClient _client1;
    private HttpClient _client2;
    private HttpClient _client3;

    private String _sortType;

    [SetUp]
    public void Setup()
    {
        UserDTO _user1Temp;
        UserDTO _user2Temp;
        UserDTO _user3Temp;
        HttpClient _client1Temp;
        HttpClient _client2Temp;
        HttpClient _client3Temp;

        (_client1Temp, _user1Temp, _) = Utils.CreateAndAuthenticateUser();
        (_client2Temp, _user2Temp, _) = Utils.CreateAndAuthenticateUser();
        (_client3Temp, _user3Temp, _) = Utils.CreateAndAuthenticateUser();

        _user1 = _user1Temp;
        _client1 = _client1Temp;
        _user2 = _user2Temp;
        _client2 = _client2Temp;
        _user3 = _user3Temp;
        _client3 = _client3Temp;

        _spool = Utils.CreateSpool(_client1, _user1.Id, null, new List<string> { _user3.Id });
        _thread = Utils.CreateThread(_client1, _user1.Id, _spool.Id, title: Utils.GetCleanUUIDString(), content: Utils.GetCleanUUIDString());
        _sortType = "new";
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
    public void GetThreadTest() {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET, _thread.Id);
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        var thread = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.That(thread, Is.Not.Null);
        Assert.That(thread.Id.Equals(_thread.Id));
        Assert.That(thread.OwnerId.Equals(_thread.OwnerId));
        Assert.That(thread.SpoolId.Equals(_thread.SpoolId));
        Assert.That(thread.Title.Equals(_thread.Title));
        Assert.That(thread.Content.Equals(_thread.Content));
    }

    [Test]
    public void GetThreadInvalidTest() {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET, "invalid");
        var result = _client1.GetAsync(endpoint).Result;
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
    public void GetAllThreadsFilteredTest()
    {
        foreach (string sortType in new string[] { "hot", "top", "controversial", "comments", "new" }) {
            var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL_FILTERED, sortType);

            var result = _client1.GetAsync(endpoint).Result;

            Assert.IsTrue(result.IsSuccessStatusCode);
            var threads = Utils.ParseResponse<List<ThreadFull>>(result);
            Assert.IsFalse(threads.IsNullOrEmpty());

            Assert.IsTrue(threads!.Any((x) => x.Id.Equals(_thread.Id)));
        }
    }

    [Test]
    public void GetAllThreadsFilteredWithQueryTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL_FILTERED + "?q=" + _thread.Title, _sortType);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads![0].Id.Equals(_thread.Id));
    }

    [Test]
    public void SpoolOwnerAndModDeleteTest()
    {
        var t1 = Utils.CreateThread(_client2, _user2.Id, _spool.Id);
        var t2 = Utils.CreateThread(_client2, _user2.Id, _spool.Id);

        var endpoint = String.Format(Endpoints.V1_THREAD_DELETE, t1.Id);
        var result = _client1.DeleteAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);

        endpoint = String.Format(Endpoints.V1_THREAD_DELETE, t2.Id);
        result = _client3.DeleteAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
    }

    [Test]
    public void DeleteThreadsInvalidTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_DELETE, _thread.Id);
        UserDTO _user3;
        HttpClient _client3;
        (_client3, _user3, _) = Utils.CreateAndAuthenticateUser();
        var result = _client3.DeleteAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);

        endpoint = String.Format(Endpoints.V1_THREAD_DELETE, Utils.GetCleanUUIDString());
        result = _client3.DeleteAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void StitchAndRipThreadTest()
    {
        // get thread
        var endpoint = String.Format(Endpoints.V1_THREAD_GET, _thread.Id);
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        ThreaditAPI.Models.Thread thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.That(thread1, Is.Not.Null);
        Assert.That(thread1.Id.Equals(_thread.Id));
        Assert.AreEqual(thread1.Stitches.Count, 0);

        // stitch thread
        endpoint = String.Format(Endpoints.V1_THREAD_STITCH);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Stitches.Count, 1);
        Assert.IsTrue(thread1.Stitches.Contains(_user1.Id));

        // stitch thread again to remove stitch
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Stitches.Count, 0);
        Assert.IsFalse(thread1.Stitches.Contains(_user1.Id));

        // rip thread
        endpoint = String.Format(Endpoints.V1_THREAD_RIP);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Rips.Count, 1);
        Assert.IsTrue(thread1.Rips.Contains(_user1.Id));

        // stitch thread (also removes rip)
        endpoint = String.Format(Endpoints.V1_THREAD_STITCH);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Rips.Count, 0);
        Assert.AreEqual(thread1.Stitches.Count, 1);
        Assert.IsFalse(thread1.Rips.Contains(_user1.Id));
        Assert.IsTrue(thread1.Stitches.Contains(_user1.Id));

        // rip thread (also removes stitch)
        endpoint = String.Format(Endpoints.V1_THREAD_RIP);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Rips.Count, 1);
        Assert.IsTrue(thread1.Rips.Contains(_user1.Id));
        Assert.IsFalse(thread1.Stitches.Contains(_user1.Id));

        // rip thread again to remove
        endpoint = String.Format(Endpoints.V1_THREAD_RIP);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(thread1.Id)).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        thread1 = Utils.ParseResponse<ThreaditAPI.Models.Thread>(result);
        Assert.AreEqual(thread1.Rips.Count, 0);
        Assert.IsFalse(thread1.Rips.Contains(_user1.Id));
    }

    [Test]
    public void StitchAndRipInvalidTest()
    {
        // stitch invalid thread id
        var endpoint = String.Format(Endpoints.V1_THREAD_STITCH);
        var result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(Utils.GetCleanUUIDString())).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);

        // rip invalid thread id
        endpoint = String.Format(Endpoints.V1_THREAD_RIP);
        result = _client1.PostAsync(endpoint,  Utils.WrapContent<String>(Utils.GetCleanUUIDString())).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void EditBadThreadTests()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_EDIT);
        ThreaditAPI.Models.Thread editedThread;

        //edit while not the owner
        editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = _thread.Id,
            OwnerId = _user1.Id,
            SpoolId = _spool.Id,
            Content = "new content"
        };

        var result = _client2.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
        //should return unauthorized
        Assert.IsFalse(result.IsSuccessStatusCode);

        //send with null
        editedThread = null!;

        result = _client2.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread!)).Result;
        //should return unauthorized
        Assert.IsFalse(result.IsSuccessStatusCode);

    // send with id being empty string
        editedThread = new ThreaditAPI.Models.Thread()
        {
            Id = " ",
            OwnerId = _user1.Id,
            SpoolId = _spool.Id,
            Content = "new content"
        };

        result = _client2.PostAsync(endpoint, Utils.WrapContent<ThreaditAPI.Models.Thread>(editedThread)).Result;
        //should return unauthorized
        Assert.IsFalse(result.IsSuccessStatusCode);
    }
}