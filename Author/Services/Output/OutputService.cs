using Serilog;
using Spire.Doc;
using TemplateCreator.Services.Config;
using TemplateCreator.Services.Csv;

namespace TemplateCreator.Services.Output
{
    public class OutputService
    {
        public static void GenerateFiles(Configuration config, IEnumerable<CsvLine> csvLines) 
        {
            Log.Information("Generating files.....");

            Parallel.ForEach(csvLines, csvLine =>
            {
                var outputFile = GetFileName(csvLine.Fields, Path.Combine(config.OutputFilesPath, config.OutputFileName));

                var document = new Document(config.DocFileTemplatePath);

                var replaced = 0;
                foreach (var field in csvLine.Fields)
                    replaced += document.Replace(field.Keyword, field.FieldValue, false, true);

                Log.Information($"Generating file {outputFile}");
                document.SaveToFile(outputFile);
            });

            Log.Information($"{csvLines.Count()} file generated.");
        }

        private static string GetFileName(List<CSVField> fields, string outputFilePath)
        {
            foreach (var field in fields)
            {
                outputFilePath = outputFilePath
                    .Replace(field.Keyword, field.FieldValue)
                    .Replace("[timestamp]", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff"));
            }

            return outputFilePath;
        }
    }
}
