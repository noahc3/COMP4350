﻿using System.Net.Cache;
using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using ThreaditAPI.Models;
using ThreaditAPI.Models.Requests;
using ThreaditAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/thread")]
    public class ThreadController : ControllerBase {
        
        [HttpGet("{threadId}")]
        public async Task<IActionResult> GetThread([FromRoute] string threadId, [FromServices] ThreadService threadService) {            
            Models.ThreadFull? threadFull = await threadService.GetThreadFullAsync(threadId);
            return Ok(threadFull);
        }

        [HttpPost("create")]
        [AuthenticationRequired]
        public async Task<IActionResult> PostThread([FromBody] PostThreadRequest request, [FromServices] ThreadService threadService) {            
            UserDTO? userDTO = Request.HttpContext.GetUser();

            if (userDTO == null) {
                return Unauthorized();
            }
            
            Models.Thread thread = new Models.Thread{
                Title = request.Title,
                Content = request.Content,
                Topic = request.Topic,
                OwnerId = userDTO.Id, 
                SpoolId = request.SpoolId
            };

            try {
                thread = await threadService.InsertThreadAsync(thread);
                return Ok(thread);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }   
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllThreads([FromServices] ThreadService threadService) {
            Models.ThreadFull[] threads = await threadService.GetAllThreadsAsync();
            return Ok(threads);
        }
    }
}
