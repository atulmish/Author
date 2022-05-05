using Serilog;
using TemplateCreator.Services.Config;

namespace TemplateCreator.Services.Csv
{
    public class CsvReader
    {
        public static IEnumerable<CsvLine> ReadFile(Configuration config)
        {
            try
            {
                Log.Information($"Reading CSV file {config.DataFilePath}");

                using var reader = new StreamReader(config.DataFilePath);

                var result = new List<CsvLine>();

                Log.Information($"Reading headers");
                var headers = reader.ReadLine()
                    .Split(config.CsvDelimiter)
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var line in headers)
                    Log.Information(line);

                Log.Information($"\nBELOW KEYWORDS WOULD BE REPLACE FROM THE TEMPLATE");
                foreach (var line in headers.Where(x => !string.IsNullOrWhiteSpace(x)))
                    Log.Information(line.ToKeyword());

                Log.Information("Reading data.");
                while (!reader.EndOfStream)
                {
                    var csvLine = new CsvLine();

                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = line.Split(config.CsvDelimiter);

                    for (int i = 0; i < headers.Count(); i++)
                    {
                        if (string.IsNullOrWhiteSpace(headers[i]))
                            continue;

                        csvLine.Fields.Add(new CSVField { FieldName = headers[i], FieldValue = values[i].Trim(), Keyword = headers[i].ToKeyword() });
                    }

                    result.Add(csvLine);
                }

                Log.Information($"Total lines of data {result.Count}");

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to read CSV file {config.DataFilePath}");
                throw;
            }
        }
    }

    public class CsvLine
    {
        public List<CSVField> Fields { get; set; } = new List<CSVField> { };
    }

    public class CSVField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Keyword { get; set; }
    }

    public static class StringToKeyword
    {
        public static string ToKeyword(this string input) 
        {
            return $"{{{input.Trim()}}}";
        }
    }
}
