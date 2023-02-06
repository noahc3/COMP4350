using ThreaditAPI.Models;

namespace ThreaditAPI.Extensions {
    public static class HttpContextExtensions {
        public static void SetUser(this HttpContext context, UserDTO user) {
            context.Items["User"] = user;
        }

        public static UserDTO? GetUser(this HttpContext context) {
            return context.Items["User"] as UserDTO;
        }
    }
}
