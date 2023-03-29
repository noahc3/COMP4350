using ThreaditAPI.Models;

namespace ThreaditAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetUser(this HttpContext context, UserDTO user)
        {
            context.Items["User"] = user;
        }

        public static UserDTO GetUser(this HttpContext context)
        {
            if (!context.Items.ContainsKey("User"))
            {
                // It should never be possible for this to happen
                // (the middleware won't run a controller if the user isn't authenticated)
                throw new Exception("User not authenticated.");
            }

            return (context.Items["User"] as UserDTO)!;
        }
    }
}
