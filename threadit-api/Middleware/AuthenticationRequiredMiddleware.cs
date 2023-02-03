using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ThreaditAPI.Constants;
using ThreaditAPI.Extensions;
using ThreaditAPI.Models;
using System.Text.Json;
using System.Security.Claims;
using ThreaditAPI.Services;
using System.Security.Cryptography.X509Certificates;

namespace ThreaditAPI.Middleware {
    public class AuthenticationRequiredMiddleware {
        private RequestDelegate _next;

        public AuthenticationRequiredMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, UserSessionService sessionService, UserService userService) {
            bool isValid = false;
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<AuthenticationRequiredAttribute>();

            if (attribute != null) {
                var sessionToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                Console.WriteLine(sessionToken);

                if (!string.IsNullOrWhiteSpace(sessionToken)) {
                    User? user = await sessionService.GetUserFromSession(sessionToken);
                    if (user != null) {
                        context.SetUserProfile(new UserProfile(user));
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
