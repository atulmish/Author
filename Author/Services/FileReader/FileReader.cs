using CsvHelper;
using Serilog;
using System.Globalization;

namespace TemplateCreator.Services.Csv
{
    public class FileReader
    {
        public static IEnumerable<CsvLine> ReadFile(string filePath, string csvDelimiter = ",")
        {
            try
            {
                Log.Information($"Reading file {filePath}");

                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Read();
                csv.ReadHeader();

                Log.Information("Keywords: {0}", csv.HeaderRecord.Select(x => x.ToKeyword()) );

                var records = csv.GetRecords<dynamic>();

                var result = records.Select(record => 
                {
                    var line = (IDictionary<string, object>)record;
                    return new CsvLine
                    {
                        Fields = csv.HeaderRecord
                          .Select(x => {
                              var value = (string)line[x];
                              return new CSVField { FieldName = x, FieldValue = value, Keyword = x.ToKeyword() };
                          })
                          .ToList()
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to read CSV file {filePath}");
                throw;
            }
        }
    }

    public record CsvLine
    {
        public List<CSVField> Fields { get; set; } = new List<CSVField> { };
    }

    public record CSVField
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;
    }

    public static class StringToKeyword
    {
        public static string ToKeyword(this string input, string prefix = "{", string suffix = "}") 
        {
            return $"{prefix.Trim()}{input.Trim()}{suffix.Trim()}";
        }
    }
}
