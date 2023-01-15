using System.Text.Json.Serialization;

namespace BudgeteerAPI.Models {
    public class UserProfile {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        [JsonIgnore]
        public User User { get; set; } = new User();
    }
}
