using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;

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
    private String _sortType;

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
        _spool2 = Utils.CreateSpool(_client2, _user2.Id);
        _thread1 = Utils.CreateThread(_client1, _user1.Id, _spool1.Id);
        _thread2 = Utils.CreateThread(_client2, _user2.Id, _spool1.Id);

        _userSettings2 = Utils.JoinSpool(_client2, _spool1.Name);
        _userSettings1 = Utils.JoinSpool(_client1, _spool2.Name);

        _sortType = "new";

    }

    [Test]
    public void GetSpoolThreadsTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL + "?spoolId=" + _spool1.Id);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads![1].Id.Equals(_thread1.Id));
        Assert.IsTrue(threads![0].Id.Equals(_thread2.Id));
    }

    [Test]
    public void GetSpoolThreads_SpoolNotExists_ShouldReturnNone()
    {
        var endpoint = Endpoints.V1_THREAD_GET_ALL + "?spoolId=" + Utils.GetCleanUUIDString();

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);

        var threads = Utils.ParseResponse<List<ThreadFull>>(result);

        Assert.That(threads.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetSpoolThreadsWithQueryTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL + "?q=" + _thread1.Title + "&spoolId=" + _spool1.Id);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads![0].Id.Equals(_thread1.Id));
    }

    [Test]
    public void GetSpoolThreadsFilteredTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL_FILTERED + "?spoolId=" + _spool1.Id, _sortType);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads![1].Id.Equals(_thread1.Id));
        Assert.IsTrue(threads![0].Id.Equals(_thread2.Id));
    }

    [Test]
    public void GetSpoolThreadsFilteredWithQueryTest()
    {
        var endpoint = String.Format(Endpoints.V1_THREAD_GET_ALL_FILTERED + "?q=" + _thread1.Title + "&spoolId=" + _spool1.Id, _sortType);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var threads = Utils.ParseResponse<List<ThreadFull>>(result);
        Assert.IsFalse(threads.IsNullOrEmpty());

        Assert.IsTrue(threads![0].Id.Equals(_thread1.Id));
    }

    [Test]
    public void CreateUpdateDeleteSpoolTest()
    {
        //create spool
        var endpoint = String.Format(Endpoints.V1_SPOOL_CREATE);
        var result = _client1.PostAsync(endpoint, Utils.WrapContent<Spool>(_spool1)).Result;

        //it was already added in setup, but lets verify we failed to add
        Assert.IsFalse(result.IsSuccessStatusCode);

        //ensure it is there though
        endpoint = String.Format(Endpoints.V1_SPOOL_GET, _spool1.Name);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);
        var spool = Utils.ParseResponse<Spool>(result);
        Assert.That(spool, Is.Not.Null);
        Assert.IsTrue(_spool1.Id.Equals(spool.Id));

        //update the spool
        endpoint = String.Format(Endpoints.V1_SPOOL_SAVE_RULES, _spool1.Id);
        var rules2 = Utils.GetCleanUUIDString();
        SaveRulesRequest req = new SaveRulesRequest()
        {
            Rules = rules2
        };

        result = _client1.PostAsync(endpoint, Utils.WrapContent<SaveRulesRequest>(req)).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);

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

        //slightly janky but for some reason there are 12 spools not 2 and they never stay in the same places. this works for now.
        bool firstFound = false;
        bool secondFound = false;
        for (int i = 0; i < spools.Length; i++)
        {
            var spool = spools[i];
            if (spool.Id == _spool1.Id)
            {
                firstFound = true;
            }
            else if (spool.Id == _spool2.Id)
            {
                secondFound = true;
            }
            if (firstFound && secondFound)
            {
                break;
            }
        }
        Assert.IsTrue(firstFound && secondFound);
    }

    [Test]
    public void GetJoinedSpoolsTest()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_JOINED, _user1.Id);
        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var spools = Utils.ParseResponse<List<Spool>>(result);
        Assert.IsFalse(spools.IsNullOrEmpty());

        //contains doesnt work. have to do this for now
        bool firstFound = false;
        bool secondFound = false;
        for (int i = 0; i < spools!.Count; i++)
        {
            var spool = spools[i];
            if (spool.Id == _spool1.Id)
            {
                firstFound = true;
            }
            else if (spool.Id == _spool2.Id)
            {
                secondFound = true;
            }
            if (firstFound && secondFound)
            {
                break;
            }
        }
        Assert.IsTrue(firstFound && secondFound);
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

        //contains wont work :'(
        bool found = false;
        for (int i = 0; i < mods!.Length; i++)
        {
            var mod = mods[i];
            if (mod.Id == _user2.Id)
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            Assert.Fail();
        }

        //add user2 as a mod again (should fail)
        endpoint = String.Format(Endpoints.V1_SPOOL_ADD_MOD, _spool1.Id, _user2.Username);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);

        //remove user2 as mod
        endpoint = String.Format(Endpoints.V1_SPOOL_REMOVE_MOD, _spool1.Id, _user2.Id);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsTrue(result.IsSuccessStatusCode);

        //check the mods again
        endpoint = String.Format(Endpoints.V1_SPOOL_MODS, _spool1.Id);
        result = _client1.GetAsync(endpoint).Result;
        mods = Utils.ParseResponse<UserDTO[]>(result);
        Assert.IsTrue(mods.IsNullOrEmpty());

        //add owner as a mod (should fail)
        endpoint = String.Format(Endpoints.V1_SPOOL_ADD_MOD, _spool1.Id, _user1.Username);
        result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void AddModerator_SpoolMissing_ShouldFail()
    {
        //add user2 as a mod
        var endpoint = String.Format(Endpoints.V1_SPOOL_ADD_MOD, Utils.GetCleanUUIDString(), _user2.Username);
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
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

    [Test]
    public void ChangeSpoolOwner_SpoolNotExists_ShouldFail()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_CHANGE_OWNER, Utils.GetCleanUUIDString(), _user2.Username);
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void ChangeSpoolOwner_AlreadyOwner_ShouldFail()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_CHANGE_OWNER, _spool1.Id, _user1.Username);
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void DeleteSpool_SpoolNotExists_ShouldFail()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_DELETE, Utils.GetCleanUUIDString());
        var result = _client1.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void DeleteSpool_NotOwner_ShouldFail()
    {
        var endpoint = String.Format(Endpoints.V1_SPOOL_DELETE, _spool1.Id);
        var result = _client2.GetAsync(endpoint).Result;
        Assert.IsFalse(result.IsSuccessStatusCode);
    }

    [Test]
    public void SuggestedSpoolsTest()
    {
        string interest = Utils.GetCleanUUIDString();
        var spool = Utils.CreateSpool(_client2, _user2.Id, new List<string> { interest });
        var endpoint = String.Format(Endpoints.V1_USERSETTINGS_ADD_INTEREST, interest);
        var result = _client1.GetAsync(endpoint).Result;

        endpoint = String.Format(Endpoints.V1_SPOOL_SUGGESTED, _user1.Id);
        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var spools = Utils.ParseResponse<Spool[]>(result);

        Assert.IsFalse(spools.IsNullOrEmpty());
        Assert.That(spools!.Any((x) => x.Id == spool.Id));
    }
}
