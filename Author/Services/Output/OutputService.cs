using NPOI.OpenXml4Net.OPC;
using NPOI.XWPF.UserModel;
using Serilog;
using System.Collections.Concurrent;
using TemplateCreator.Services.Csv;

namespace TemplateCreator.Services.Output
{
    public class OutputService
    {
        public static void GenerateFiles(Config.Configuration config, IEnumerable<CsvLine> csvLines) 
        {
            Log.Information("Generating files.....");

            var filesGenerated = new ConcurrentQueue<CsvLine>();
            Parallel.ForEach(csvLines, new ParallelOptions { MaxDegreeOfParallelism = 1 }, csvLine =>
            {
                try
                {
                    GenerateFile(config, csvLine);
                    filesGenerated.Enqueue(csvLine);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Failed to generate file");
                }
            });

            Log.Information($"{filesGenerated.Count()}/{csvLines.Count()} file generated.");
        }

        private static void GenerateFile(Config.Configuration config, CsvLine csvLine) 
        {
            var doc = new XWPFDocument(OPCPackage.Open(config.Source.TemplateFile));

            ReplaceTextinParagraph(doc.Paragraphs, csvLine.Fields);

            foreach (XWPFTable tbl in doc.Tables)
                foreach (XWPFTableRow row in tbl.Rows)
                    foreach (XWPFTableCell cell in row.GetTableCells())
                        ReplaceTextinParagraph(cell.Paragraphs, csvLine.Fields);

            var fileName = GetFileName(csvLine.Fields, config.Output.Filename) + "." + config.Output.Format.ToLower();
            var filePath = Path.Combine(config.Output.Directory, fileName);

            doc.Write(new FileStream(filePath, FileMode.Create, FileAccess.Write));

            Log.Information($"File generated {filePath}");
        }

        private static string GetFileName(List<CSVField> fields, string outputFilePath)
        {
            foreach (var field in fields)
            {
                outputFilePath = outputFilePath
                    .Replace(field.Keyword, field.FieldValue)
                    .Replace("[id]", NUlid.Ulid.NewUlid().ToString()); ;
            }

            return outputFilePath;
        }

        private static void ReplaceTextinParagraph(IList<XWPFParagraph> paragraphs, List<CSVField> fields) 
        {
            foreach (XWPFParagraph p in paragraphs)
                foreach (var field in fields)
                    p.ReplaceText(field.Keyword, field.FieldValue);
        }
    }
}
