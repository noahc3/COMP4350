﻿using ThreaditAPI.Extensions;
using ThreaditAPI.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace ThreaditAPI.Controllers.v1 {
    [ApiController]
    [Route("v1/user")]
    public class UserController : ControllerBase {
        [HttpGet("profile")]
        [AuthenticationRequired]
        public IActionResult GetProfile() {
            return Ok(Request.HttpContext.GetUserProfile());
        }
    }
}