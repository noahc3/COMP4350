using BudgeteerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using PemUtils;

namespace BudgeteerAPI.Constants {
    public class ExternalServicesConstants {
        public static string DBUSER = Environment.GetEnvironmentVariable("dbUser") ?? "";
        public static string DBPASSWORD = Environment.GetEnvironmentVariable("dbPassword") ?? "";
        public static string DBNAME = Environment.GetEnvironmentVariable("dbName") ?? "";
        public static string DBHOST = Environment.GetEnvironmentVariable("dbHost") ?? "";

        public static Uri OIDC_CONFIG_URL = new Uri("https://budgeteer-auth.noahc3.ml/application/o/budgeteer/.well-known/openid-configuration");
        public static string OIDC_CLIENT_ID = "f05aa98327aceb762ab7c7c83d67760dc96f976a";
        public static string OIDC_PK_PATH = "budgeteer-jwt-certificate_private_key.pem";
        public static RsaSecurityKey OIDC_PK = loadKey();

        private static RsaSecurityKey loadKey() {
            using (var stream = File.OpenRead(OIDC_PK_PATH))
            using (var reader = new PemReader(stream)) {
                return new RsaSecurityKey(reader.ReadRsaKey());
            }
        }

        public static AuthSource GetAuthSourceByIssuer(string issuer)
        {
            switch (issuer)
            {
                default: return AuthSource.AUTHENTIK;
            }
        }  
    }
}
