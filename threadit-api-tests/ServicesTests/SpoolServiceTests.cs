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
    private string longRules = "luctus venenatis lectus magna fringilla urna porttitor rhoncus dolor purus non enim praesent elementum facilisis leo vel fringilla est ullamcorper eget nulla facilisi etiam dignissim diam quis enim lobortis scelerisque fermentum dui faucibus in ornare quam viverra orci sagittis eu volutpat odio facilisis mauris sit amet massa vitae tortor condimentum lacinia quis vel eros donec ac odio tempor orci dapibus ultrices in iaculis nunc sed augue lacus viverra vitae congue eu consequat ac felis donec et odio pellentesque diam volutpat commodo sed egestas egestas fringilla phasellus faucibus scelerisque eleifend donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit dignissim sodales ut eu sem integer vitae justo eget magna fermentum iaculis eu non diam phasellus vestibulum lorem sed risus ultricies tristique nulla aliquet enim tortor at auctor urna nunc id cursus metus aliquam eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus semper eget duis at tellus at urna condimentum mattis pellentesque id nibh tortor id aliquet lectus proin nibh nisl condimentum id venenatis a condimentum vitae sapien pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas sed tempus urna et pharetra pharetra massa massa ultricies mi quis hendrerit dolor magna eget est lorem ipsum dolor sit amet consectetur adipiscing elit pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas integer eget aliquet nibh praesent tristique magna sit amet purus gravida quis blandit turpis cursus in hac habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat mauris nunc congue nisi vitae suscipit tellus mauris a diam maecenas sed enim ut sem viverra aliquet eget sit amet tellus cras adipiscing enim eu turpis egestas pretium aenean pharetra magna ac placerat vestibulum lectus mauris ultrices eros in cursus turpis massa tincidunt dui ut ornare lectus sit amet est placerat in egestas erat imperdiet sed euismod nisi porta lorem mollis aliquam ut porttitor leo a diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper sit amet risus nullam eget felis eget nunc lobortis mattis aliquam faucibus purus inplacerat in egestas erat imperdiet sed euismod nisi porta lorem mollis aliquam ut porttitor leo a diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper sit amet risus nullam eget felis eget nunc lobortis mattis aliquam faucibus purus in";


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
    public async Task RetrieveSpoolByName_Invalid_ShouldFail()
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

        // null name
        var spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "",
            OwnerId = testUser.Id,
        };
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.InsertSpoolAsync(spool));

        // name not only letters or numbers
        spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool1!?",
            OwnerId = testUser.Id,
        };
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.InsertSpoolAsync(spool));

        // spaces in name
        spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool 1",
            OwnerId = testUser.Id,
        };
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.InsertSpoolAsync(spool));

        // rules has more than 2048 chars
        spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool1",
            OwnerId = testUser.Id,
            Rules = longRules
        };
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.InsertSpoolAsync(spool));

        // name over 32 chars
        spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "qr5t9c5190314e9bb7126df32cd75641234523457632612345678909876543",
            OwnerId = testUser.Id,
        };
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.InsertSpoolAsync(spool));

        //save rules that are too long
        //owner
        User testUser2 = new User()
        {
            Id = "d94ddc51-9031-4e9b-b712-6df32cd75642",
            Username = "testUser1",
            Email = "testUser1@test.com"
        };
        // Ensure User is not in database
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);
        Assert.That(returnedUser, Is.Null);
        // Add User to database
        await _userRepository.InsertUserAsync(testUser2);
        returnedUser = await _userRepository.GetUserByLoginIdentifierAsync(testUser2.Username);

        // Create Spool
        spool = new Spool()
        {
            Id = "qr5t9c51-9031-4e9b-b712-6df32cd75641",
            Name = "Spool1",
            OwnerId = testUser2.Id,
        };

        // Ensure spool is not in database
        Assert.ThrowsAsync<Exception>(async () => await _spoolService.GetSpoolByNameAsync(spool.Name));

        // Insert spool into database
        await _spoolService.InsertSpoolAsync(spool);

        Assert.ThrowsAsync<Exception>(async () => await _spoolService.SaveRulesAsync(spool.Id, longRules));
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
