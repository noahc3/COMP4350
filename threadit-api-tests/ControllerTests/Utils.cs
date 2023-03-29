using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ThreaditAPI;
using ThreaditAPI.Constants;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;

public static class Utils
{
    private static WebApplicationFactory<Program>? _factory;
    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public static WebApplicationFactory<Program> GetWebApplicationFactory()
    {
        if (_factory == null)
        {
            var context = new PostgresDbContext();
            context.Database.EnsureDeleted();
            context.SaveChanges();
            _factory = new WebApplicationFactory<Program>();
        }

        return _factory;
    }

    public static HttpClient GetHttpClient()
    {
        var factory = GetWebApplicationFactory();
        return factory.CreateClient();
    }

    public static T? ParseResponse<T>(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<T>(content, serializerOptions);
    }

    public static HttpContent WrapContent<T>(T content)
    {
        return new StringContent(JsonSerializer.Serialize<T>(content), Encoding.UTF8, "application/json");
    }

    public static (HttpClient, UserDTO, string) CreateAndAuthenticateUser()
    {
        CreateAccountRequest reqCreate = new CreateAccountRequest()
        {
            Email = GetCleanUUIDString() + "@test.com",
            Password = "password",
            Username = GetCleanUUIDString()
        };

        LoginRequest loginReq = new LoginRequest()
        {
            Username = reqCreate.Email,
            Password = reqCreate.Password
        };

        var client = GetHttpClient();

        var response = client.PostAsync(Endpoints.V1_AUTH_REGISTER, WrapContent(reqCreate)).Result;

        var loginResponse = client.PostAsync(Endpoints.V1_AUTH_LOGIN, WrapContent(reqCreate)).Result;
        var token = loginResponse.Content.ReadAsStringAsync().Result;

        CheckSessionRequest checkReq = new CheckSessionRequest()
        {
            Token = token
        };

        var checkResponse = client.PostAsync(Endpoints.V1_AUTH_CHECKSESSION, WrapContent(checkReq)).Result;

        if (checkResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Could not authenticate user");
        }

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        var profileResponse = client.GetAsync(Endpoints.V1_USER_PROFILE).Result;

        var profile = ParseResponse<UserDTO>(profileResponse);

        if (profile == null)
        {
            throw new Exception("Could not get user profile");
        }

        if (profile.Username != reqCreate.Username)
        {
            throw new Exception("User profile does not match");
        }

        return (client, profile, token);
    }

    public static Spool CreateSpool(HttpClient authenticatedClient, string ownerId, List<string>? interests = null, List<string>? moderators = null)
    {
        if (interests == null)
        {
            interests = new List<string> { "keyword" };
        }

        if (moderators == null)
        {
            moderators = new List<string>();
        }

        PostSpoolRequest req = new PostSpoolRequest()
        {
            Name = GetCleanUUIDString(),
            Interests = interests,
            Moderators = moderators,
            OwnerId = ownerId
        };

        var response = authenticatedClient.PostAsync(Endpoints.V1_SPOOL_CREATE, WrapContent(req)).Result;

        var spool = ParseResponse<Spool>(response);

        if (spool == null)
        {
            throw new Exception("Could not create spool");
        }

        return spool;
    }

    public static void DeleteSpool(HttpClient authenticatedClient, string spoolId)
    {
        var response = authenticatedClient.GetAsync(String.Format(Endpoints.V1_SPOOL_DELETE, spoolId)).Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Could not delete spool");
        }
    }

    public static ThreaditAPI.Models.Thread CreateThread(HttpClient authenticatedClient, string ownerId, string spoolId, string? title = null, string? content = null, string? type = null)
    {
        if (title == null)
        {
            title = GetCleanUUIDString();
        }

        if (content == null)
        {
            content = GetCleanUUIDString();
        }

        if (type == null)
        {
            type = ThreadTypes.TEXT;
        }

        PostThreadRequest req = new PostThreadRequest()
        {
            Title = title,
            Content = content,
            OwnerId = ownerId,
            SpoolId = spoolId,
            ThreadType = type
        };

        var response = authenticatedClient.PostAsync(Endpoints.V1_THREAD_CREATE, WrapContent(req)).Result;

        var thread = ParseResponse<ThreaditAPI.Models.Thread>(response);

        if (thread == null)
        {
            throw new Exception("Could not create thread");
        }

        return thread;
    }

    public static void DeleteThread(HttpClient authenticatedClient, string threadId)
    {
        var response = authenticatedClient.DeleteAsync(String.Format(Endpoints.V1_THREAD_DELETE, threadId)).Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Could not delete thread");
        }
    }

    public static Comment CreateComment(HttpClient authenticatedClient, string ownerId, string threadId, string? content = null)
    {
        if (content == null)
        {
            content = GetCleanUUIDString();
        }

        var endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, threadId, "");

        var result = authenticatedClient.PostAsync(endpoint, Utils.WrapContent<string>(content)).Result;
        var comment = Utils.ParseResponse<Comment>(result);

        if (comment == null)
        {
            throw new Exception("Could not create comment");
        }

        return comment;
    }

    public static UserSettings JoinSpool(HttpClient authenticatedClient, string spoolName)
    {
        var response = authenticatedClient.GetAsync(String.Format(Endpoints.V1_USERSETTINGS_JOIN, spoolName)).Result;

        var settings = ParseResponse<UserSettings>(response);

        if (settings == null)
        {
            throw new Exception("Could not join spool");
        }

        return settings;
    }

    public static string GetCleanUUIDString()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}
