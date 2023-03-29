using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;
using ThreaditAPI.Services;

namespace ThreaditAPI.Controllers.v1
{
    [ApiController]
    [Route("v1/comments")]
    public class CommentsController : ControllerBase
    {

        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetBaseComments([FromRoute] string threadId, [FromServices] CommentService commentService)
        {
            CommentFull[] comments = await commentService.GetBaseComments(threadId);
            return Ok(comments);
        }

        [HttpGet("{threadId}/expand/{parentCommentId}")]
        public async Task<IActionResult> ExpandComments([FromRoute] string threadId, [FromRoute] string parentCommentId, [FromServices] CommentService commentService)
        {
            CommentFull[] comments = await commentService.ExpandComments(threadId, parentCommentId);
            return Ok(comments);
        }

        [HttpGet("{threadId}/older/{siblingCommentId}")]
        public async Task<IActionResult> OlderComments([FromRoute] string threadId, [FromRoute] string siblingCommentId, [FromServices] CommentService commentService)
        {
            CommentFull[] comments = await commentService.OlderComments(threadId, siblingCommentId);
            return Ok(comments);
        }

        [HttpGet("{threadId}/newer/{siblingCommentId}")]
        public async Task<IActionResult> NewerComments([FromRoute] string threadId, [FromRoute] string siblingCommentId, [FromServices] CommentService commentService)
        {
            CommentFull[] comments = await commentService.NewerComments(threadId, siblingCommentId);
            return Ok(comments);
        }

        [HttpPost("{threadId}/{parentCommentId?}")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostComment([FromRoute] string threadId, [FromRoute] string? parentCommentId, [FromBody] string commentContent, [FromServices] CommentService commentService)
        {
            UserDTO user = Request.HttpContext.GetUser();

            Comment comment = new Comment
            {
                ThreadId = threadId,
                ParentCommentId = parentCommentId,
                OwnerId = user.Id,
                Content = commentContent
            };

            Comment insertedComment = await commentService.InsertCommentAsync(comment);

            return Ok(insertedComment);
        }

        [HttpPatch("{threadId}/edit")]
        [AuthenticationRequired]
        public async Task<IActionResult> EditComment([FromRoute] string threadId, [FromBody] Comment comment, [FromServices] CommentService commentService)
        {
            UserDTO user = Request.HttpContext.GetUser();

            Comment editedComment = await commentService.UpdateCommentAsync(user.Id, comment);

            return Ok(editedComment);
        }

        [HttpDelete("{threadId}/{commentId}")]
        [AuthenticationRequired]
        public async Task<IActionResult> DeleteComment([FromRoute] string threadId, [FromRoute] string commentId, [FromServices] CommentService commentService)
        {
            UserDTO user = Request.HttpContext.GetUser();

            Comment deletedComment = await commentService.DeleteCommentAsync(user.Id, commentId);

            return Ok(deletedComment);
        }


    }
}
