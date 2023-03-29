using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;

namespace ThreaditTests.Controllers;
public class UserControllerTests
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
        _spool2 = Utils.CreateSpool(_client2, _user2.Id);
        _thread1 = Utils.CreateThread(_client1, _user1.Id, _spool1.Id);

        _userSettings2 = Utils.JoinSpool(_client2, _spool1.Name);
        _userSettings1 = Utils.JoinSpool(_client1, _spool2.Name);

        _thread2 = Utils.CreateThread(_client2, _user2.Id, _spool1.Id);
    }

    [Test]
    public void GetLogoutGetUserTest()
    {
        //get the profile
        var endpoint = String.Format(Endpoints.V1_USER_PROFILE);

        var result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);
        var loggedInUser = Utils.ParseResponse<UserDTO>(result);
        Assert.IsTrue(loggedInUser!.Id.Equals(_user1.Id));

        //logout
        endpoint = String.Format(Endpoints.V1_AUTH_LOGOUT);

        result = _client1.GetAsync(endpoint).Result;

        Assert.IsTrue(result.IsSuccessStatusCode);

        //get the profile again
        endpoint = String.Format(Endpoints.V1_USER_PROFILE);

        try
        {
            //will throw error that there is not user logged in/authenticated
            result = _client1.GetAsync(endpoint).Result;
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }
}
