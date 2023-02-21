using ThreaditAPI.Constants;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Database {
    public class DbInitializer {
        public static void Initialize(PostgresDbContext context) {
            if (context.Spools.Any()) {
                return;
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hash = BCrypt.Net.BCrypt.HashPassword("testPassword", salt);
            var users = new User[] {
            new User { Id = "00000000-0000-456a-b0f7-7a8c172c23e0", Email = "test@gmail.com", Username = "testAccount", PasswordHash = hash},
                new User { Id = "3257a727-3e9c-4734-808e-42ff9725c779", Email = "test@gmail.com", Username = "stevenpost", PasswordHash = hash},
                new User { Id = "c55330ec-0977-4d01-8137-8cab28a2d7f6", Email = "test@gmail.com", Username = "LuinAelin", PasswordHash = "testPassword"},
                new User { Id = "74b990e7-eed6-4cff-9769-5fbbf2fcaf56", Email = "test@gmail.com", Username = "Ichbinian", PasswordHash = "testPassword"},
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var spools = new Spool[] {
                new Spool { Id = "7f527ccf-a2bc-4adb-a7da-970be1175525", Name = "hockey", Interests = new List<string> { "Hockey" }, OwnerId = "00000000-0000-456a-b0f7-7a8c172c23e0" },
                new Spool { Id = "40cf2fd7-4c7b-422e-a9e7-3ee689e4d68c", Name = "AskThreadit", Interests = new List<string> { "Community" }, OwnerId = "00000000-0000-456a-b0f7-7a8c172c23e0" }
            };

            context.Spools.AddRange(spools);
            context.SaveChanges();

            var threads = new Models.Thread[] {
                new Models.Thread { 
                    Id = "65976296-d7db-497c-b2a2-36af72c325b1",
                    Title = "What's the greatest episode of a tv show ever made?",
                    OwnerId = "3257a727-3e9c-4734-808e-42ff9725c779",
                    SpoolId = "40cf2fd7-4c7b-422e-a9e7-3ee689e4d68c",
                    Content = "This needs at least some content lol",
                    DateCreated = DateTime.Parse("2023-02-15T10:16:39+00:00").ToUniversalTime()
                },
                new Models.Thread { 
                    Id = "6649b329-44d8-49ed-be89-a9ce56d35d04",
                    Title = "Who's the worst main character we're supposed to sympathise with?",
                    OwnerId = "c55330ec-0977-4d01-8137-8cab28a2d7f6",
                    SpoolId = "40cf2fd7-4c7b-422e-a9e7-3ee689e4d68c",
                    Content = "Yay more content!",
                    DateCreated = DateTime.Parse("2023-02-10T11:45:52+00:00").ToUniversalTime()
                },
                new Models.Thread {
                    Id = "4c60da7e-a692-44b3-acbb-7619db366d10",
                    Title = "Would you rather Nurse for 8 years at $9.25M or Karlsson at $11.5M for 4 years?",
                    OwnerId = "74b990e7-eed6-4cff-9769-5fbbf2fcaf56",
                    SpoolId = "7f527ccf-a2bc-4adb-a7da-970be1175525",
                    Content = "Personally, Karlsson. I think Nurse is overpaid and overvalued for what he brings to the ice.",
                    DateCreated = DateTime.Parse("2023-02-15T17:50:55+00:00").ToUniversalTime()
                }
            };

            context.Threads.AddRange(threads);
            context.SaveChanges();

            var userSettings = new UserSettings[] {
                new UserSettings {
                    Id = "00000000-0000-456a-b0f7-7a8c172c23e0",
                    DarkMode = false,
                    SpoolsJoined = new List<string> {
                        "7f527ccf-a2bc-4adb-a7da-970be1175525",
                        "40cf2fd7-4c7b-422e-a9e7-3ee689e4d68c"
                    }
                },
                new UserSettings {
                    Id = "3257a727-3e9c-4734-808e-42ff9725c779",
                    DarkMode = false,
                    SpoolsJoined = new List<string> {
                        "7f527ccf-a2bc-4adb-a7da-970be1175525"
                    }
                }
            };

            context.UserSettings.AddRange(userSettings);
            context.SaveChanges();
        }
    }
}
