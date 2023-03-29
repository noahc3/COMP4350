using System;

namespace ThreaditAPI.Constants
{
    public class ExternalServicesConstants
    {
        public static string DB_USER = Environment.GetEnvironmentVariable("dbUser") ?? "threadit";
        public static string DB_PASSWORD = Environment.GetEnvironmentVariable("dbPassword") ?? "1234";
        public static string DB_NAME = Environment.GetEnvironmentVariable("dbName") ?? "threadit-tests";
        public static string DB_HOST = Environment.GetEnvironmentVariable("dbHost") ?? "localhost:5999";
    }
}
