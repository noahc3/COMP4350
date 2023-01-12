namespace BudgeteerAPI.Models {
    public class User {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    }
}
