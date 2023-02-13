
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            app.UseMiddleware<AuthenticationRequiredMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}