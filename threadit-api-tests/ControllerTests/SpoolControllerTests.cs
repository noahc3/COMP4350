using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace ThreaditTests.Controllers;
public class SpoolControllerTests
{
    private Spool _spool1;
    private Spool _spool2;
    private ThreaditAPI.Models.Thread _thread1;
    private ThreaditAPI.Models.Thread _thread2;
    private UserDTO _user1;
    private UserDTO _user2;
    private HttpClient _client1;
    private HttpClient _client2;
    private UserSettings _userSettings1;
    private UserSettings _userSettings2;

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
        _user2 = _user2Temp;
        _client1 = _client1Temp;
        _client2 = _client2Temp;

        _spool1 = Utils.CreateSpool(_client1, _user1.Id);
        _thread1 = Utils.CreateThread(_client1, _user1.Id, _spool1.Id);

        _userSettings2 = Utils.JoinSpool(_client2Temp, _user2.Id);

        _thread2 = Utils.CreateThread(_client2, _user2.Id, _spool1.Id);
    }

    [Test]
    public void GetSpoolThreadsTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_GET_THREADS, _spool1.Id);

        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads!.Contains(_thread1));
        Assert.IsTrue(threads!.Contains(_thread2));
    }

    [Test]
    public void CreateUpdateDeleteSpoolTest()
    {
        //create spool
        var endpoint = String.Format(Endpoints.V1_SPOOL_CREATE);
        var result = _client1.PostAsync(endpoint, Utils.WrapContent<Spool>(_spool1)).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var returnedSpool = Utils.ParseResponse<Spool>(result);
        Assert.IsNotNull(returnedSpool);
        Assert.IsTrue(returnedSpool.OwnerId.Equals(_user1.Id));
        Assert.IsTrue(returnedSpool.Rules.Equals(_spool1.Rules));

        //update the spool
        endpoint = String.Format(Endpoints.V1_SPOOL_SAVE_RULES, _spool1.Id);
        var rules2 = Utils.GetCleanUUIDString();

        result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(rules2)).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        returnedSpool = Utils.ParseResponse<Spool>(result);
        Assert.IsNotNull(returnedSpool);

        Assert.IsTrue(returnedSpool.Rules.Equals(rules2));
        Assert.IsTrue(returnedSpool.OwnerId.Equals(_user1.Id));

        //delete the spool
        endpoint = String.Format(Endpoints.V1_SPOOL_DELETE, _spool1.Id);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);

        //ensure it is gone
        endpoint = String.Format(Endpoints.V1_SPOOL_GET, _spool1.Name);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void GetAllSpoolsTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_GET_ALL);
        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var spools = Utils.ParseResponse<Spool[]>(result);

        Assert.IsFalse(spools.IsNullOrEmpty());
        Assert.IsTrue(spools!.Contains(_spool1));
        Assert.IsTrue(spools!.Contains(_spool2));
    }

    [Test]
    public void GetJoinedSpoolsTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_JOINED, _user1.Id);
        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var spools = Utils.ParseResponse<List<Spool>>(result);
        Assert.IsFalse(spools.IsNullOrEmpty());

        Assert.IsTrue(spools!.Contains(_spool1));
        Assert.IsFalse(spools!.Contains(_spool2));
    }

    [Test]
    public void GetAddRemoveSpoolModeratorsTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_MODS, _spool1.Id);
        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var mods = Utils.ParseResponse<UserDTO[]>(result);

        Assert.IsTrue(mods.IsNullOrEmpty());

        //add user2 as a mod
        endpoint = String.Format(Endpoints.V1_SPOOL_ADD_MOD, _spool1.Id, _user2.Username);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);

        //check the mods again
        endpoint = String.Format(Endpoints.V1_SPOOL_MODS, _spool1.Id);
        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        mods = Utils.ParseResponse<UserDTO[]>(result);

        Assert.IsFalse(mods.IsNullOrEmpty());
        Assert.IsTrue(mods.Contains(_user2));

        //remove user2 as mod
        endpoint = String.Format(Endpoints.V1_SPOOL_REMOVE_MOD, _spool1.Id, _user2.Id);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);

        //check the mods again
        endpoint = String.Format(Endpoints.V1_SPOOL_MODS, _spool1.Id);
        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(mods.IsNullOrEmpty());
    }

    [Test]
    public void ChangeSpoolOwnerTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_CHANGE_OWNER, _spool1.Id, _user2.Username);
        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var spool = Utils.ParseResponse<Spool>(result);

        Assert.IsTrue(_user2.Id == spool!.OwnerId);
        Assert.IsTrue(_spool1.Id == spool!.Id);
    }
}