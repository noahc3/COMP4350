using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BudgeteerAPI.Models {
    public enum AuthSource {
        [EnumMember(Value = "Authentik")] AUTHENTIK = 0
    }

    [PrimaryKey(nameof(AuthId), nameof(AuthSource))]
    public class UserAuthLink {
        public string AuthId { get; set; } = "";
        public string UserId { get; set; } = "";
        public AuthSource AuthSource { get; set; }
    }
}