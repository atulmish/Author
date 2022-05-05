using Serilog;
using TemplateCreator.Services.Config;
using TemplateCreator.Services.Csv;
using TemplateCreator.Services.Output;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

try
{
    Log.Information("Authoring started");
    var config = ConfigService.ReadConfig();
    var csvLines = CsvReader.ReadFile(config);
    OutputService.GenerateFiles(config, csvLines);
    Log.Information("Authoring completed");
}
catch (Exception ex)
{
    Log.Error(ex, "Authoring Failed");
}
finally
{
    Log.CloseAndFlush();
    Console.ReadKey();
}

