using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
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

	[Test]
	public void InterestsTest()
	{
		string interest1 = "interest1";
		string interest2 = "interest2";

		// add interest1, ensure its in the result, ensure its in the usersettings interests list, 
		// ensure belongs says user belongs to interest1
		var endpoint = String.Format(Endpoints.V1_USERSETTINGS_ADD_INTEREST, interest1);
		var result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var interests = Utils.ParseResponse<string[]>(result);
		Assert.IsTrue(interests.Contains(interest1));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_INTERESTS);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsTrue(interests.Contains(interest1));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest1);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsTrue(belong);

		// add interest2, ensure its in the result, ensure its in the usersettings interests list, 
		// ensure belongs says user belongs to interest1 and interest2
		endpoint = String.Format(Endpoints.V1_USERSETTINGS_ADD_INTEREST, interest2);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsTrue(interests.Contains(interest1));
		Assert.IsTrue(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_INTERESTS);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsTrue(interests.Contains(interest1));
		Assert.IsTrue(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest1);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsTrue(belong);

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest2);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsTrue(belong);

		// remove interest1, ensure its not in the result, ensure its not in the usersettings interests list, 
		// ensure belongs says user does not belong to interest1 but does to interest2
		endpoint = String.Format(Endpoints.V1_USERSETTINGS_REMOVE_INTEREST, interest1);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsFalse(interests.Contains(interest1));
		Assert.IsTrue(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_INTERESTS);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsFalse(interests.Contains(interest1));
		Assert.IsTrue(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest1);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsFalse(belong);

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest2);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsTrue(belong);

		// remove interest2, ensure its not in the result, ensure its not in the usersettings interests list, 
		// ensure belongs says user does not belong to interest1 or interest2
		endpoint = String.Format(Endpoints.V1_USERSETTINGS_REMOVE_INTEREST, interest2);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsFalse(interests.Contains(interest1));
		Assert.IsFalse(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_INTERESTS);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		interests = Utils.ParseResponse<string[]>(result);
		Assert.IsFalse(interests.Contains(interest1));
		Assert.IsFalse(interests.Contains(interest2));

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest1);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsFalse(belong);

		endpoint = String.Format(Endpoints.V1_USERSETTINGS_BELONG_INTEREST, interest2);
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		belong = bool.Parse(result.Content.ReadAsStringAsync().Result);
		Assert.IsFalse(belong);
	}

	[Test]
	public void GetInvalidUserSettingsTest()
	{

	}
}
