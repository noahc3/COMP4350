
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ThreaditAPI.Middleware;
using ThreaditAPI.Constants;
using ThreaditAPI.Database;
using ThreaditAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "cors",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000");
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    });
            });

            builder.Services.AddDbContext<PostgresDbContext>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserSessionService>();
            builder.Services.AddScoped<ThreadService>();
            builder.Services.AddScoped<UserSettingsService>();
            builder.Services.AddScoped<SpoolService>();
            builder.Services.AddScoped<CommentService>();
            builder.Services.AddScoped<InterestService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.OperationFilter<OptionalRouteParameterOperationFilter>();
            });
            builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumMemberConverter()));
            builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumMemberConverter()));

            var app = builder.Build();

            app.UseCors("cors");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<PostgresDbContext>();
                context.Database.Migrate();
                DbInitializer.Initialize(context);
            }

            app.UseMiddleware<AuthenticationRequiredMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

    public class OptionalRouteParameterOperationFilter : IOperationFilter
    {
        const string captureName = "routeParameter";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpMethodAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>();

            var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template?.Contains("?") ?? false);
            if (httpMethodWithOptional == null)
                return;

            string regex = $"{{(?<{captureName}>\\w+)\\?}}";

            var matches = System.Text.RegularExpressions.Regex.Matches(httpMethodWithOptional.Template!, regex);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var name = match.Groups[captureName].Value;

                var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
                if (parameter != null)
                {
                    parameter.AllowEmptyValue = true;
                    parameter.Description = "Must check \"Send empty value\" or Swagger passes a comma for empty values otherwise";
                    parameter.Required = false;
                    //parameter.Schema.Default = new OpenApiString(string.Empty);
                    parameter.Schema.Nullable = true;
                }
            }
        }
    }
}