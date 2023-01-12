using BudgeteerAPI.Models;
using BudgeteerAPI.Utilities.CsvConverters;
using BudgeteerAPI.Utilities.TransactionParsers;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace BudgeteerAPI.Utilities.TransactionParsers {
    public class CIBCTransactionParser : ITransactionParser {

        public CIBCTransactionParser() {

        }

        public Transaction[] ParseTransactions(byte[] data) {
            string text = System.Text.Encoding.UTF8.GetString(data);
            text = "date,description,debit,credit,card\r\n" + text;
            using (var reader = new StringReader(text))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csv.Context.RegisterClassMap<CibcTransactionMap>();
                return csv.GetRecords<Transaction>().ToArray();
            }
        }

        public class CibcTransactionMap : ClassMap<Transaction> {
            public CibcTransactionMap() {
                Map(m => m.Date).Name("date");
                Map(m => m.BankDescription).Name("description");
                Map(m => m.Debit).Name("debit").TypeConverter<FloatIntFiatConverter>();
                Map(m => m.Credit).Name("credit").TypeConverter<FloatIntFiatConverter>();
            }
        }
    }
}
