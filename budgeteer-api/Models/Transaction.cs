using Microsoft.Extensions.Caching.Memory;

namespace BudgeteerAPI.Models {
    public class Transaction {
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string BankDescription { get; set; } = "";
        public string Description { get; set; } = "";
        public int Debit { get; set; }
        public int Credit { get; set; }
        public string Category { get; set; } = "";
    }
}
