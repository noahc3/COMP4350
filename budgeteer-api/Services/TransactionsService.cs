using BudgeteerAPI.Models;
using BudgeteerAPI.Utilities.TransactionParsers;

namespace BudgeteerAPI.Services {
    public enum TransactionImportSource {
        CIBC_CSV_PERSONAL
    }

    public class TransactionsService {
        public static Transaction[] ImportTransactions(TransactionImportSource source, byte[] data) {
            Transaction[] transactions;

            switch (source) {
                case TransactionImportSource.CIBC_CSV_PERSONAL:
                    CIBCTransactionParser parser = new CIBCTransactionParser();
                    transactions = parser.ParseTransactions(data);
                    break;
                default:
                    transactions = Array.Empty<Transaction>();
                    break;
            }

            return transactions;
        }
    }
}
