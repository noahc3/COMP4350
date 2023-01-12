using BudgeteerAPI.Models;

namespace BudgeteerAPI.Extensions {
    public static class HttpContextExtensions {
        public static void SetUserProfile(this HttpContext context, UserProfile profile) {
            context.Items["UserProfile"] = profile;
        }

        public static UserProfile? GetUserProfile(this HttpContext context) {
            return context.Items["UserProfile"] as UserProfile;
        }
    }
}
