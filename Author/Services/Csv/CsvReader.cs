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

                var headers = reader.ReadLine().Split(config.CsvDelimiter);

                while (!reader.EndOfStream)
                {
                    var csvLine = new CsvLine();

                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = line.Split(config.CsvDelimiter);

                    for (int i = 0; i < headers.Count(); i++)
                    {
                        var keyword = $"{{{headers[i].Trim()}}}";
                        csvLine.Fields.Add(new CSVField { FieldName = headers[i].Trim(), FieldValue = values[i].Trim(), Keyword = keyword });
                    }

                    result.Add(csvLine);
                }

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
}
