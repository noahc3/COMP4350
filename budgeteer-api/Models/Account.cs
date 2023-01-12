using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BudgeteerAPI.Models {

    public enum AccountType {
        [EnumMember(Value = "Chequing")] CHEQUING,
        [EnumMember(Value = "Savings")] SAVINGS,
        [EnumMember(Value = "Credit Card")] CREDIT_CARD
    }

    public enum AccountBank {
        [EnumMember(Value = "CIBC (Personal)")] CIBC_PERSONAL,
        [EnumMember(Value = "PayPal (Personal)")] PAYPAL_PERSONAL,
        [EnumMember(Value = "Stack")] STACK
    }

    public class Account {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = "";

        public string Name { get; set; } = "";

        public AccountType Type { get; set; }

        public AccountBank Bank { get; set; }

        public DateTime LastImport { get; set; }

        [NotMapped]
        public long Balance { get; set; }

    }
}
