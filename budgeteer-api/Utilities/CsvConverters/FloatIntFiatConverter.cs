using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BudgeteerAPI.Utilities.CsvConverters {
    public class FloatIntFiatConverter : DefaultTypeConverter {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData) {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return (int) Math.Floor(Convert.ToDouble(text) * 100.0);
        }

        public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData) {
            value ??= 0;
            return (((double)value) / 100.0).ToString();
        }
    }
}
