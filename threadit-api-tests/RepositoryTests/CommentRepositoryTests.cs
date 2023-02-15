using Microsoft.EntityFrameworkCore;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditTests.Repositories;

public class CommentRepositoryTests
{
    private CommentRepository _commentRepository;
    private PostgresDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new PostgresDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
        _commentRepository = new CommentRepository(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task RetrieveComment_NotExists_ShouldFail()
    {
        Comment testComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };
        Comment? returnedComment = await _commentRepository.GetCommentAsync(testComment);

        Assert.That(returnedComment, Is.Null);
    }

    [Test]
    public async Task RetrieveCommentById_NotExists_ShouldFail()
    {
        Comment? returnedComment = await _commentRepository.GetCommentAsync("bdf89c51-9031-4e9b-b712-6df32cd75641");

        Assert.That(returnedComment, Is.Null);
    }

    [Test]
    public async Task RetrieveComment_Exists_ShouldPass()
    {
        // Create Comment
        Comment testComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };

        // Ensure Comment is not in database
        Comment? returnedComment = await _commentRepository.GetCommentAsync(testComment);
        Assert.That(returnedComment, Is.Null);

        // Add Comment to database
        await _commentRepository.InsertCommentAsync(testComment);
        returnedComment = await _commentRepository.GetCommentAsync(testComment);

        // Ensure Comment is added correctly
        Assert.That(returnedComment, Is.Not.Null);
        Assert.IsTrue(returnedComment.Id.Equals(testComment.Id));
        Assert.IsTrue(returnedComment.Content.Equals(testComment.Content));
        Assert.IsTrue(returnedComment.OwnerId.Equals(testComment.OwnerId));
        Assert.IsTrue(returnedComment.ThreadId.Equals(testComment.ThreadId));
        Assert.That(returnedComment.ParentCommentId, Is.Null);
    }

    [Test]
    public async Task RetrieveCommentById_Exists_ShouldPass()
    {
        // Create Comment
        Comment testComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };

        // Ensure Comment is not in database
        Comment? returnedComment = await _commentRepository.GetCommentAsync(testComment.Id);
        Assert.That(returnedComment, Is.Null);

        // Add Comment to database
        await _commentRepository.InsertCommentAsync(testComment);
        returnedComment = await _commentRepository.GetCommentAsync(testComment.Id);

        // Ensure Comment is added correctly
        Assert.That(returnedComment, Is.Not.Null);
        Assert.IsTrue(returnedComment.Id.Equals(testComment.Id));
        Assert.IsTrue(returnedComment.Content.Equals(testComment.Content));
        Assert.IsTrue(returnedComment.OwnerId.Equals(testComment.OwnerId));
        Assert.IsTrue(returnedComment.ThreadId.Equals(testComment.ThreadId));
        Assert.That(returnedComment.ParentCommentId, Is.Null);
    }

    [Test]
    public async Task UpdateComment_Exists_ShouldPass()
    {
        // Create Comment
        Comment testComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };

        // Ensure Comment is not in database
        Comment? returnedComment = await _commentRepository.GetCommentAsync(testComment.Id);
        Assert.That(returnedComment, Is.Null);

        // Add Comment to database
        await _commentRepository.InsertCommentAsync(testComment);
        returnedComment = await _commentRepository.GetCommentAsync(testComment.Id);

        // Ensure Comment is added correctly
        Assert.That(returnedComment, Is.Not.Null);
        Assert.IsTrue(returnedComment.Id.Equals(testComment.Id));
        Assert.IsTrue(returnedComment.Content.Equals(testComment.Content));
        Assert.IsTrue(returnedComment.OwnerId.Equals(testComment.OwnerId));
        Assert.IsTrue(returnedComment.ThreadId.Equals(testComment.ThreadId));
        Assert.That(returnedComment.ParentCommentId, Is.Null);

        //Create updated Comment
        Comment updatedTestComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Update Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };

        //update comment in the database
        Comment? updateReturnedComment = await _commentRepository.UpdateCommentAsync(updatedTestComment);

        //get the comment again
        returnedComment = await _commentRepository.GetCommentAsync(updatedTestComment.Id);

        //make sure comment has been updated properly
        Assert.That(returnedComment, Is.Not.Null);
        Assert.IsTrue(returnedComment.Id.Equals(updatedTestComment.Id));
        Assert.IsTrue(returnedComment.Content.Equals(updatedTestComment.Content));
        Assert.IsTrue(returnedComment.OwnerId.Equals(updatedTestComment.OwnerId));
        Assert.IsTrue(returnedComment.ThreadId.Equals(updatedTestComment.ThreadId));
    }

    [Test]
    public async Task UpdateComment_NotExists_ShouldPass()
    {
        // Create Comment
        Comment testComment = new Comment()
        {
            Id = "64a59d99-c16c-4016-b113-25a2f62cf51f",
            Content = "Comment Content",
            OwnerId = "a2k6n2b6-0000-4016-b113-25a2f62cf51f",
            ThreadId = "823ae618-0b9d-4f35-a3e4-9514b5651dbb",
            ParentCommentId = null
        };

        // Ensure Thread is not in database
        Comment? returnedThread = await _commentRepository.GetCommentAsync(testComment.Id);
        Assert.That(returnedThread, Is.Null);

        //update comment
        Comment? updateReturnedComment = await _commentRepository.UpdateCommentAsync(testComment);

        //get the comment again
        returnedThread = await _commentRepository.GetCommentAsync(testComment.Id);

        //make sure it was not in the database
        Assert.That(returnedThread, Is.Null);
    }
}