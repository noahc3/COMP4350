using System;

namespace ThreaditAPI.Models {
    public class UserSession {
        public string Id { get; set; } = Guid.NewGuid().ToString(); //note: not cryptographically secure, should use something else
        public string UserId {get;set;} = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateExpires { get; set; } = DateTime.UtcNow.AddDays(30);
    }
}
