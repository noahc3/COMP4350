using System;

namespace ThreaditAPI.Constants {
    public class ExternalServicesConstants {
        public static string DB_USER = Environment.GetEnvironmentVariable("dbUser") ?? "";
        public static string DB_PASSWORD = Environment.GetEnvironmentVariable("dbPassword") ?? "";
        public static string DB_NAME = Environment.GetEnvironmentVariable("dbName") ?? "";
        public static string DB_HOST = Environment.GetEnvironmentVariable("dbHost") ?? "";
    }
}
