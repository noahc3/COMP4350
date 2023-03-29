using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ThreaditAPI;
using ThreaditAPI.Database;
using ThreaditAPI.Models;

namespace ThreaditTests.Controllers;

public class CommentsControllerTests
{
	private Spool _spool;
	private ThreaditAPI.Models.Thread _thread;
	private UserDTO _user1;
	private UserDTO _user2;
	private UserDTO _user3;
	private HttpClient _client1;
	private HttpClient _client2;
	private HttpClient _client3;

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
		_user2 = _user2Temp;
		_user3 = _user3Temp;
		_client1 = _client1Temp;
		_client2 = _client2Temp;
		_client3 = _client3Temp;

		_spool = Utils.CreateSpool(_client1, _user1.Id, null, new List<string> { _user3.Id });
		_thread = Utils.CreateThread(_client1, _user1.Id, _spool.Id);
	}

	[Test]
	public void CreateUpdateDeleteCommentTest()
	{
		var endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, _thread.Id, "");

		string commentText1 = Utils.GetCleanUUIDString();
		var result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(commentText1)).Result;

		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment1 = Utils.ParseResponse<Comment>(result);
		Assert.That(comment1, Is.Not.Null);
		Assert.IsTrue(commentText1.Equals(comment1.Content));

		string commentText2 = Utils.GetCleanUUIDString();
		result = _client2.PostAsync(endpoint, Utils.WrapContent<string>(commentText2)).Result;

		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment2 = Utils.ParseResponse<Comment>(result);
		Assert.That(comment2, Is.Not.Null);
		Assert.IsTrue(commentText2.Equals(comment2.Content));

		endpoint = String.Format(Endpoints.V1_COMMENT_EDIT, _thread.Id);

		comment1.Content = Utils.GetCleanUUIDString();

		result = _client1.PatchAsync(endpoint, Utils.WrapContent<Comment>(comment1)).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment1Updated = Utils.ParseResponse<Comment>(result);
		Assert.That(comment1Updated, Is.Not.Null);
		Assert.IsTrue(comment1.Content.Equals(comment1Updated.Content));

		result = _client2.PatchAsync(endpoint, Utils.WrapContent<Comment>(comment1)).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);

		endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, comment1.Id);
		result = _client2.DeleteAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);

		endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, comment2.Id);
		result = _client2.DeleteAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment2Deleted = Utils.ParseResponse<Comment>(result);
		Assert.That(comment2Deleted, Is.Not.Null);
		Assert.That(comment2Deleted.Id, Is.EqualTo(comment2.Id));
		Assert.That(comment2Deleted.Content, Is.EqualTo("[deleted]"));
	}

	[Test]
	public void ModeratorAndSpoolOwnerDelete()
	{
		Comment c1 = Utils.CreateComment(_client2, _user1.Id, _thread.Id);
		Comment c2 = Utils.CreateComment(_client2, _user2.Id, _thread.Id);

		var endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, c1.Id);
		var result = _client3.DeleteAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment1Deleted = Utils.ParseResponse<Comment>(result);
		Assert.That(comment1Deleted, Is.Not.Null);
		Assert.That(comment1Deleted.Id, Is.EqualTo(c1.Id));
		Assert.That(comment1Deleted.Content, Is.EqualTo("[deleted]"));

		endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, c2.Id);
		result = _client1.DeleteAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var comment2Deleted = Utils.ParseResponse<Comment>(result);
		Assert.That(comment2Deleted, Is.Not.Null);
		Assert.That(comment2Deleted.Id, Is.EqualTo(c2.Id));
		Assert.That(comment2Deleted.Content, Is.EqualTo("[deleted]"));
	}

	[Test]
	public void LoadThreadCommentsTest()
	{
		HttpResponseMessage result;
		var endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, _thread.Id, "");
		var comments = new List<Comment>();

		for (int i = 0; i < 30; i++)
		{
			string text = Utils.GetCleanUUIDString();
			result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(text)).Result;
			Assert.IsTrue(result.IsSuccessStatusCode);
			var comment = Utils.ParseResponse<Comment>(result);
			Assert.That(comment, Is.Not.Null);
			Assert.That(text, Is.EqualTo(comment.Content));

			comments.Add(comment);
		}

		comments = comments.OrderByDescending(c => c.DateCreated).ToList();

		endpoint = String.Format(Endpoints.V1_COMMENT_BASE, _thread.Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(10));

		for (int i = 0; i < 10; i++)
		{
			Assert.IsTrue(comments[i].Id == commentsResponse[i].Id);
		}

		endpoint = String.Format(Endpoints.V1_COMMENT_OLDER, _thread.Id, comments[9].Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(11));

		for (int i = 0; i < 11; i++)
		{
			Assert.IsTrue(comments[i + 9].Id == commentsResponse[i].Id);
		}

		endpoint = String.Format(Endpoints.V1_COMMENT_OLDER, _thread.Id, comments[19].Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(11));

		for (int i = 0; i < 11; i++)
		{
			Assert.IsTrue(comments[i + 19].Id == commentsResponse[i].Id);
		}

		endpoint = String.Format(Endpoints.V1_COMMENT_OLDER, _thread.Id, comments[29].Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(1));

		endpoint = String.Format(Endpoints.V1_COMMENT_NEWER, _thread.Id, comments[0].Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(1));

		endpoint = String.Format(Endpoints.V1_COMMENT_NEWER, _thread.Id, comments[5].Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(6));

		for (int i = 0; i < 6; i++)
		{
			Assert.IsTrue(comments[i].Id == commentsResponse[i].Id);
		}
	}

	[Test]
	public void ExpandComments()
	{
		HttpResponseMessage result;
		string endpoint;
		var comments = new List<Comment>();

		for (int i = 0; i < 30; i++)
		{
			endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, _thread.Id, "");
			string text = Utils.GetCleanUUIDString();
			result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(text)).Result;
			Assert.IsTrue(result.IsSuccessStatusCode);
			var comment = Utils.ParseResponse<Comment>(result);
			Assert.That(comment, Is.Not.Null);
			Assert.That(text, Is.EqualTo(comment.Content));

			comments.Add(comment);

			for (int j = 0; j < (i % 3) + 5; j++)
			{
				string replyText = Utils.GetCleanUUIDString();
				endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, _thread.Id, comment.Id);
				result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(replyText)).Result;
				Assert.IsTrue(result.IsSuccessStatusCode);
				var reply = Utils.ParseResponse<Comment>(result);
				Assert.That(reply, Is.Not.Null);

				comments.Add(reply);

				for (int k = 0; k < 2; k++)
				{
					string replyText2 = Utils.GetCleanUUIDString();
					endpoint = String.Format(Endpoints.V1_COMMENT_CREATE, _thread.Id, reply.Id);
					result = _client1.PostAsync(endpoint, Utils.WrapContent<string>(replyText2)).Result;
					Assert.IsTrue(result.IsSuccessStatusCode);
					var reply2 = Utils.ParseResponse<Comment>(result);
					Assert.That(reply2, Is.Not.Null);

					comments.Add(reply2);
				}
			}
		}

		endpoint = String.Format(Endpoints.V1_COMMENT_BASE, _thread.Id);

		result = _client1.GetAsync(endpoint).Result;
		Assert.IsTrue(result.IsSuccessStatusCode);
		var commentsResponse = Utils.ParseResponse<List<Comment>>(result);
		Assert.That(commentsResponse, Is.Not.Null);
		Assert.That(commentsResponse.Count, Is.EqualTo(10 + (10 * 2)));

		foreach (Comment c in commentsResponse)
		{
			Assert.IsTrue(comments.Any(x => x.Id == c.Id));
		}

		foreach (Comment c in commentsResponse.Where((x) => x.ParentCommentId != null))
		{
			Assert.IsTrue(comments.Any(x => x.Id == c.ParentCommentId));
			Assert.IsTrue(commentsResponse.Any(x => x.Id == c.ParentCommentId));

			endpoint = String.Format(Endpoints.V1_COMMENT_EXPAND, _thread.Id, c.Id);

			result = _client1.GetAsync(endpoint).Result;
			Assert.IsTrue(result.IsSuccessStatusCode);
			var commentsResponse2 = Utils.ParseResponse<List<Comment>>(result);
			Assert.That(commentsResponse2, Is.Not.Null);
			Assert.That(commentsResponse2.Count, Is.EqualTo(3));

			foreach (Comment c2 in commentsResponse2)
			{
				Assert.IsTrue(comments.Any(x => x.Id == c2.Id));
			}

			Assert.That(commentsResponse2.Count((x) => x.ParentCommentId == c.Id), Is.EqualTo(2));
		}

		foreach (Comment c in commentsResponse.Where((x) => x.ParentCommentId == null))
		{
			var oldestReply = commentsResponse.Where((x) => x.ParentCommentId == c.Id).OrderBy((x) => x.DateCreated).First();

			endpoint = String.Format(Endpoints.V1_COMMENT_OLDER, _thread.Id, oldestReply.Id);

			result = _client1.GetAsync(endpoint).Result;
			Assert.IsTrue(result.IsSuccessStatusCode);
			var commentsResponse2 = Utils.ParseResponse<List<Comment>>(result);
			Assert.That(commentsResponse2, Is.Not.Null);
			Assert.That(commentsResponse2.Count, Is.EqualTo(4));

			foreach (Comment c2 in commentsResponse2)
			{
				Assert.IsTrue(comments.Any(x => x.Id == c2.Id));
			}
		}
	}

	[Test]
	public void DeleteComment_ThreadMissing_ShouldFail()
	{
		Comment comment = Utils.CreateComment(_client1, _user1.Id, _thread.Id);
		Assert.That(comment, Is.Not.Null);

		Utils.DeleteThread(_client1, _thread.Id);

		string endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, comment.Id);
		var result = _client1.DeleteAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);
	}

	[Test]
	public void DeleteComment_SpoolMissing_ShouldFail()
	{
		Comment comment = Utils.CreateComment(_client1, _user1.Id, _thread.Id);
		Assert.That(comment, Is.Not.Null);

		Utils.DeleteSpool(_client1, _spool.Id);

		string endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, comment.Id);
		var result = _client1.DeleteAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);
	}

	[Test]
	public void DeleteComment_NotExists_ShouldFail()
	{
		string endpoint = String.Format(Endpoints.V1_COMMENT_DELETE, _thread.Id, Utils.GetCleanUUIDString());
		var result = _client1.DeleteAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);
	}

	[Test]
	public void UpdateComment_NotExists_ShouldFail()
	{
		string endpoint = String.Format(Endpoints.V1_COMMENT_EDIT, _thread.Id);
		var result = _client1.PatchAsync(endpoint, Utils.WrapContent<Comment>(new Comment()
		{
			OwnerId = _user1.Id,
			Id = Utils.GetCleanUUIDString(),
			Content = Utils.GetCleanUUIDString(),
			ParentCommentId = null,
			ThreadId = _thread.Id
		})).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);
	}

	[Test]
	public void InvalidSiblingCommentTests()
	{
		string endpoint = String.Format(Endpoints.V1_COMMENT_OLDER, _thread.Id, Utils.GetCleanUUIDString());
		var result = _client1.GetAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);

		endpoint = String.Format(Endpoints.V1_COMMENT_NEWER, _thread.Id, Utils.GetCleanUUIDString());
		result = _client1.GetAsync(endpoint).Result;
		Assert.IsFalse(result.IsSuccessStatusCode);
	}

}
