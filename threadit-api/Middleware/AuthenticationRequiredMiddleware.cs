using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using ThreaditAPI.Extensions;
using ThreaditAPI.Models;
using ThreaditAPI.Services;

namespace ThreaditAPI.Middleware
{
	public class AuthenticationRequiredMiddleware
	{
		private RequestDelegate _next;

		public AuthenticationRequiredMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IConfiguration configuration, UserSessionService sessionService, UserService userService)
		{
			bool isValid = false;
			var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
			var attribute = endpoint?.Metadata.GetMetadata<AuthenticationRequiredAttribute>();

			if (attribute != null)
			{
				var sessionToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

				if (!string.IsNullOrWhiteSpace(sessionToken))
				{
					UserDTO? user = await sessionService.GetUserFromSession(sessionToken);
					if (user != null)
					{
						context.SetUser(user);
						isValid = true;
					}
				}

				if (!isValid)
				{
					System.Diagnostics.Debug.WriteLine("NOT VALID");
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					return;
				}
			}

			await _next(context);
		}
	}
}
