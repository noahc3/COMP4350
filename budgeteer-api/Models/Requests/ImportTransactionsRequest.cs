using BudgeteerAPI.Services;
using System.Text.Json.Serialization;

namespace BudgeteerAPI.Models.Requests {
    public class ImportTransactionsRequest {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionImportSource source { get; set; }
        public byte[]? data { get; set; }
    }
}
