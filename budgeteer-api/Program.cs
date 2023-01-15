
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using BudgeteerAPI.Middleware;
using BudgeteerAPI.Constants;
using BudgeteerAPI.Database;
using BudgeteerAPI.Services;

namespace BudgeteerAPI {
    public class Program {
        public static void Main(string[] args) {
            var scheme = new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OpenIdConnect,
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oidc" },
                OpenIdConnectUrl = ExternalServicesConstants.OIDC_CONFIG_URL
            };

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options => {
                options.AddPolicy(
                    name: "cors",
                    builder => {
                        builder.WithOrigins("http://localhost:3000");
                        builder.WithOrigins("https://budgeteer.noahc3.ml");
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    });
            });

            builder.Services.AddDbContext<BudgeteerDbContext>();
            builder.Services.AddScoped<UserAuthLinkService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AccountService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("oidc", scheme);
                options.OperationFilter<AuthRequirementFilter>(scheme);
            });
            builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumMemberConverter()));
            builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumMemberConverter()));

            var app = builder.Build();

            app.UseCors("cors");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.OAuthClientId(ExternalServicesConstants.OIDC_CLIENT_ID);
                });
            }

            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<BudgeteerDbContext>();
                context.Database.EnsureCreated();
            }

                app.UseMiddleware<AuthenticationRequiredMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        internal class AuthRequirementFilter : IOperationFilter {
            OpenApiSecurityScheme scheme;
            public AuthRequirementFilter(OpenApiSecurityScheme scheme) {
                this.scheme = scheme;
            }

            public void Apply(OpenApiOperation operation, OperationFilterContext context) {
                if (!context.ApiDescription
                    .ActionDescriptor
                    .EndpointMetadata
                    .OfType<AuthenticationRequiredAttribute>()
                    .Any()) {
                    return;
                }

                operation.Security.Add(new OpenApiSecurityRequirement() {
                    [scheme] = new List<string>()
                });
            }
        }
    }
}