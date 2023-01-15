using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BudgeteerAPI.Constants;
using BudgeteerAPI.Extensions;
using BudgeteerAPI.Models;
using System.Text.Json;
using System.Security.Claims;
using BudgeteerAPI.Services;
using System.Security.Cryptography.X509Certificates;

namespace BudgeteerAPI.Middleware {
    public class AuthenticationRequiredMiddleware {
        private RequestDelegate _next;

        public AuthenticationRequiredMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, UserAuthLinkService ualService, UserService userService) {
            bool isValid = false;
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<AuthenticationRequiredAttribute>();

            if (attribute != null) {
                var idToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                Console.WriteLine(idToken);

                if (!string.IsNullOrWhiteSpace(idToken)) {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    try {
                        TokenValidationResult res = await tokenHandler.ValidateTokenAsync(idToken, new TokenValidationParameters {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = ExternalServicesConstants.OIDC_PK,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ClockSkew = TimeSpan.Zero
                        });

                        System.Diagnostics.Debug.WriteLine($"Valid: {res.IsValid}");

                        if (res.IsValid) {
                            string issuer = res.Issuer;
                            string authId = res.Claims[ClaimTypes.NameIdentifier] as string ?? "";
                            AuthSource source = ExternalServicesConstants.GetAuthSourceByIssuer(issuer);

                            User user = await userService.GetOrCreateUserAsync(ualService, authId, source);

                            context.SetUserProfile(new UserProfile {
                                Id = user.Id,
                                Email = res.Claims[ClaimTypes.Email] as string ?? "",
                                User = user
                            });

                            isValid = true;
                        }
                    } catch (Exception e) {
                        System.Diagnostics.Debug.WriteLine(e);
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
