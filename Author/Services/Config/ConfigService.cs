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

                config.SetDefaults();

                return config;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error with config.yml file.");
                throw;
            }
        }
    }

    public record Configuration
    {
        public Source Source { get; set; } = new Source();
        public Output Output { get; set; } = new Output();

        public void Validate() 
        {
            if (!File.Exists(Source.TemplateFile))
                throw new InvalidOperationException("The source template file doesn't exist.");

            if (!File.Exists(Source.DataFile))
                throw new InvalidOperationException("The source data file doesn't exist.");
        }

        public void SetDefaults()
        {
            if (string.IsNullOrWhiteSpace(Output.Directory))
            {
                Output.Directory = Path.Combine(Environment.CurrentDirectory, "Output");
                if(!Directory.Exists("Output.Directory"))
                    Directory.CreateDirectory(Output.Directory);
            }

            if (string.IsNullOrWhiteSpace(Output.Filename))
                Output.Filename = "[id]-[timestamp]";

            if (string.IsNullOrWhiteSpace(Output.Format))
                Output.Format = "DOCX";
        }
    }

    public record Output
    {
        public string? Directory { get; set; }
        public string? Filename { get; set; }
        public string? Format { get; set; }
    }

    public record Source
    {
        public string? DataFile { get; set; }
        public string? TemplateFile { get; set; }
    }
}
