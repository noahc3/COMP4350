using Microsoft.IdentityModel.Tokens;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;
using ThreaditAPI.Services;

namespace ThreaditTests.Repositories;

public class SpoolServiceTests
{
	private SpoolService _spoolService;
	private PostgresDbContext _dbContext;
	private UserRepository _userRepository;

	[SetUp]
	public void Setup()
	{
		_dbContext = CommonUtils.GetDbContext();
		_spoolService = new SpoolService(_dbContext);
		_userRepository = new UserRepository(_dbContext);
	}

	[Test]
	public void RetrieveSpoolByName_NotExists_ShouldFail()
	{
		Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync("doesNotExistSpool"));
	}

	[Test]
	public async Task RetrieveSpoolByName_Exists_ShouldPass()
	{
		//owner
		User testUser = new User()
		{
			Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
			Username = "testUser",
			Email = "testUser@test.com"
		};
		// Ensure User is not in database
		UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
		Assert.That(returnedUser, Is.Null);
		// Add User to database
		await _userRepository.InsertUserAsync(testUser);
		returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

		// Create Spool
		var spool = new Spool()
		{
			Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
			Name = "Spool1",
			OwnerId = testUser.Id,
		};

		// Ensure spool is not in database
		Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync(spool.Name));

		// Insert spool into database
		await _spoolService.InsertSpoolAsync(spool);

		// Retrieve spool from database
		var retrievedSpool = await _spoolService.GetSpoolByNameAsync(spool.Name);

		Assert.NotNull(retrievedSpool);
		Assert.That(retrievedSpool!.Id, Is.EqualTo(spool.Id));
		Assert.That(retrievedSpool.Name, Is.EqualTo(spool.Name));
		Assert.That(retrievedSpool.OwnerId, Is.EqualTo(spool.OwnerId));
	}

	[Test]
	public async Task RetrieveAllSpools_ShouldPass()
	{
		//owner
		User testUser = new User()
		{
			Id = "d94ddc51-9031-4e9b-b712-6df32cd75641",
			Username = "testUser",
			Email = "testUser@test.com"
		};
		// Ensure User is not in database
		UserDTO? returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);
		Assert.That(returnedUser, Is.Null);
		// Add User to database
		await _userRepository.InsertUserAsync(testUser);
		returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser.Username);

		// Create Spools
		var spools = new List<Spool>() {
			new Spool() {
				Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
				Name = "Spool1",
				OwnerId = testUser.Id,
				Interests = new List<string>() { "Interest1", "Interest2" },
				Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
			},
			new Spool() {
				Id = "adadada-9031-4e9b-b712-6df32cd75641",
				Name = "Spool2",
				OwnerId = "d94ddc51-9031-4e9b-b712-6df32cd75641",
				Interests = new List<string>() { "Interest1", "Interest2" },
				Moderators = new List<string>() { "d94ddc51-9031-4e9b-b712-6df32cd75641" }
			}
		};

		//add them to the db
		foreach (var spool in spools)
		{
			await _spoolService.InsertSpoolAsync(spool);
		}

		Spool[] returnedSpools = await _spoolService.GetAllSpoolsAsync();

		Assert.False(returnedSpools.IsNullOrEmpty());
		Assert.True(returnedSpools[0].Id.Equals("qr5t9c51-9031-4e9b-b712-6df32cd75641"));
		Assert.True(returnedSpools[0].Name.Equals("Spool1"));
		Assert.True(returnedSpools[1].Id.Equals("adadada-9031-4e9b-b712-6df32cd75641"));
		Assert.True(returnedSpools[1].Name.Equals("Spool2"));
	}
}
