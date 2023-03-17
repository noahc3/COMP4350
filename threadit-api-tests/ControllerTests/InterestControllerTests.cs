using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Models.Requests;

namespace ThreaditTests.Controllers;
public class InterestControllerTests
{
    private UserDTO _user;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        UserDTO _user1Temp;
        HttpClient _client1Temp;

        (_client1Temp, _user1Temp, _) = Utils.CreateAndAuthenticateUser();
        _user = _user1Temp;
        _client = _client1Temp;
    }

    [Test]
    public void AddRemoveInterestsTest()
    {
        string interestName1 = Utils.GetCleanUUIDString();
        string interestName2 = Utils.GetCleanUUIDString();

        var response = _client.GetAsync(Endpoints.V1_INTEREST_ALL).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        List<Interest> interests = Utils.ParseResponse<List<Interest>>(response)!;
        int defaultInterestCount = interests.Count;

        response = _client.GetAsync(String.Format(Endpoints.V1_INTEREST_ADD, interestName1)).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        response = _client.GetAsync(Endpoints.V1_INTEREST_ALL).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        interests = Utils.ParseResponse<List<Interest>>(response)!;
        Assert.That(interests.Count, Is.EqualTo(defaultInterestCount + 1));
        Assert.IsTrue(interests.Any(x => x.Name == interestName1));

        response = _client.GetAsync(String.Format(Endpoints.V1_INTEREST_ADD, interestName2)).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        response = _client.GetAsync(Endpoints.V1_INTEREST_ALL).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        interests = Utils.ParseResponse<List<Interest>>(response)!;
        Assert.That(interests.Count, Is.EqualTo(defaultInterestCount + 2));
        Assert.IsTrue(interests.Any(x => x.Name == interestName1));
        Assert.IsTrue(interests.Any(x => x.Name == interestName2));

        response = _client.GetAsync(String.Format(Endpoints.V1_INTEREST_REMOVE, interestName1)).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        response = _client.GetAsync(Endpoints.V1_INTEREST_ALL).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        interests = Utils.ParseResponse<List<Interest>>(response)!;
        Assert.That(interests.Count, Is.EqualTo(defaultInterestCount + 1));
        Assert.IsTrue(interests.Any(x => x.Name == interestName2));

        response = _client.GetAsync(String.Format(Endpoints.V1_INTEREST_REMOVE, interestName2)).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);

        response = _client.GetAsync(Endpoints.V1_INTEREST_ALL).Result;
        Assert.IsTrue(response.IsSuccessStatusCode);
        interests = Utils.ParseResponse<List<Interest>>(response)!;
        Assert.That(interests.Count, Is.EqualTo(defaultInterestCount));
    }
}