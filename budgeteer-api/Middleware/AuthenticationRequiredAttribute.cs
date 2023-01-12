namespace BudgeteerAPI.Middleware {
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthenticationRequiredAttribute : Attribute {
        public AuthenticationRequiredAttribute() { }
    }
}
