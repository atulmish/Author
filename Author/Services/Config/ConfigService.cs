using Serilog;
using YamlDotNet.Serialization;

namespace TemplateCreator.Services.Config
{
    public class ConfigService
    {
        public static Configuration ReadConfig() 
        {
            try
            {
                var configPath = Path.Combine(Environment.CurrentDirectory, "config.yml");

                Log.Information($"Reading config {configPath}");
                var file = File.ReadAllText(configPath);
                Log.Information($"Config {configPath} read successfully.");

                Log.Information($"Deserialising {configPath}.");
                var config =  new
                    DeserializerBuilder()
                    .Build()
                    .Deserialize<Configuration>(file);

                Log.Information($"Validating {configPath}.");
                config.Validate();

                return config;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error with config.yml file.");
                throw;
            }
        }
    }

    public class Configuration 
    {
        public string DocFileTemplatePath { get; set; }
        public string DataFilePath { get; set; }
        public string OutputFilesPath { get; set; }
        public string OutputFileName { get; set; }
        public string CsvDelimiter { get; set; }

        public void Validate() 
        {
            if (!File.Exists(DocFileTemplatePath))
                throw new InvalidOperationException("Template word file does not exists.");

            if (!File.Exists(DataFilePath))
                throw new InvalidOperationException("Data csv file does not exists.");

            if (!Directory.Exists(OutputFilesPath))
                Directory.CreateDirectory(OutputFilesPath);

            if (string.IsNullOrWhiteSpace(OutputFileName))
                throw new InvalidOperationException("Output file name is required.");

            if (string.IsNullOrWhiteSpace(CsvDelimiter))
                throw new InvalidOperationException("CsvDelimiter is required.");
        }
    }
}
