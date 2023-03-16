using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Models.Requests;

namespace ThreaditTests.Controllers;
public class UserSettingsControllerTests
{
    private Spool _spool1;
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

        _spool1 = Utils.CreateSpool(_client2, _user2.Id);
    }

    [Test]
    public void CheckJoinLeaveSpoolUserSettingsTest()
    {
        //check to make sure the user has not already joined the spool
        var endpoint = String.Format(Endpoints.V1_USERSETTINGS_CHECK, _spool1.Name);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var returnedValue = bool.Parse(result.Content.ReadAsStringAsync().Result);
        Assert.IsFalse(returnedValue);

        //join the spool
        endpoint = String.Format(Endpoints.V1_USERSETTINGS_JOIN, _spool1.Name);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var settings = Utils.ParseResponse<UserSettings>(result);
        Assert.IsTrue(settings!.SpoolsJoined.Contains(_spool1.Id));

        //check to make sure the user has now joined the spool
        endpoint = String.Format(Endpoints.V1_USERSETTINGS_CHECK, _spool1.Name);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        returnedValue = bool.Parse(result.Content.ReadAsStringAsync().Result);
        Assert.IsTrue(returnedValue);

        //leave the spool
        endpoint = String.Format(Endpoints.V1_USERSETTINGS_REMOVE, _spool1.Name);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        settings = Utils.ParseResponse<UserSettings>(result);
        Assert.IsFalse(settings!.SpoolsJoined.Contains(_spool1.Id));

        //check to make sure the user is not in the spool now
        endpoint = String.Format(Endpoints.V1_USERSETTINGS_CHECK, _spool1.Name);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        returnedValue = bool.Parse(result.Content.ReadAsStringAsync().Result);
        Assert.IsFalse(returnedValue);
    }
}