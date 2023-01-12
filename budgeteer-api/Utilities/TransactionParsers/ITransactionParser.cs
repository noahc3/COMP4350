using BudgeteerAPI.Models;

namespace BudgeteerAPI.Utilities.TransactionParsers {
    public interface ITransactionParser {
        Transaction[] ParseTransactions(byte[] data);
    }
}
