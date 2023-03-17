using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Models.Requests;

namespace ThreaditTests.Controllers;
public class AuthControllerTests
{

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void RegisterLoginLogoutFlowTest()
    {
        HttpClient client = Utils.GetHttpClient();

        string password = Utils.GetCleanUUIDString();
        CreateAccountRequest req = new CreateAccountRequest() {
            Email = Utils.GetCleanUUIDString() + "@test.com",
            Username = Utils.GetCleanUUIDString(),
            Password = password,
            ConfirmPassword = password
        };

        var response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json")).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        LoginRequest loginReq = new LoginRequest() {
            Username = req.Username,
            Password = req.Password
        };

        response = client.PostAsync(Endpoints.V1_AUTH_LOGIN, new StringContent(JsonSerializer.Serialize(loginReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        string token = response.Content.ReadAsStringAsync().Result;

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        CheckSessionRequest checkReq = new CheckSessionRequest() {
            Token = token
        };

        response = client.PostAsync(Endpoints.V1_AUTH_CHECKSESSION, new StringContent(JsonSerializer.Serialize(checkReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        response = client.GetAsync(Endpoints.V1_AUTH_LOGOUT).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        response = client.PostAsync(Endpoints.V1_AUTH_CHECKSESSION, new StringContent(JsonSerializer.Serialize(checkReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);
    }

    [Test]
    public void UserAlreadyExists_ShouldFail()
    {
        HttpClient client = Utils.GetHttpClient();

        string password = Utils.GetCleanUUIDString();
        CreateAccountRequest req = new CreateAccountRequest() {
            Email = Utils.GetCleanUUIDString() + "@test.com",
            Username = Utils.GetCleanUUIDString(),
            Password = password,
            ConfirmPassword = password
        };

        var response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json")).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        // email already exists
        password = Utils.GetCleanUUIDString();
        CreateAccountRequest req2 = new CreateAccountRequest() {
            Email = req.Email,
            Username = Utils.GetCleanUUIDString(),
            Password = password,
            ConfirmPassword = password
        };

        response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, new StringContent(JsonSerializer.Serialize(req2), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);

        // username already exists
        password = Utils.GetCleanUUIDString();
        CreateAccountRequest req3 = new CreateAccountRequest() {
            Email = Utils.GetCleanUUIDString(),
            Username = req.Username,
            Password = password,
            ConfirmPassword = password
        };

        response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, new StringContent(JsonSerializer.Serialize(req3), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);
    }

    [Test]
    public void InvalidLogins_ShouldFail()
    {
        HttpClient client = Utils.GetHttpClient();

        string password = Utils.GetCleanUUIDString();
        CreateAccountRequest req = new CreateAccountRequest() {
            Email = Utils.GetCleanUUIDString() + "@test.com",
            Username = Utils.GetCleanUUIDString(),
            Password = password,
            ConfirmPassword = password
        };

        var response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json")).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        // invalid username, correct password
        LoginRequest loginReq = new LoginRequest() {
            Username = Utils.GetCleanUUIDString(),
            Password = req.Password
        };

        response = client.PostAsync(Endpoints.V1_AUTH_LOGIN, new StringContent(JsonSerializer.Serialize(loginReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);

        // correct username, invalid password
        loginReq = new LoginRequest() {
            Username = req.Username,
            Password = Utils.GetCleanUUIDString()
        };

        response = client.PostAsync(Endpoints.V1_AUTH_LOGIN, new StringContent(JsonSerializer.Serialize(loginReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);

        // correct email, invalid password
        loginReq = new LoginRequest() {
            Username = req.Email,
            Password = Utils.GetCleanUUIDString()
        };

        response = client.PostAsync(Endpoints.V1_AUTH_LOGIN, new StringContent(JsonSerializer.Serialize(loginReq), Encoding.UTF8, "application/json")).Result;
        Assert.IsFalse(response.IsSuccessStatusCode);
    }
}