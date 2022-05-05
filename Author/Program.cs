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
    Log.Information("Authoring Started");
    var config = ConfigService.ReadConfig();
    var csvLines = CsvReader.ReadFile(config);
    OutputService.GenerateFiles(config, csvLines);
    Log.Information("Author exited with errors");
}
catch (Exception ex)
{
    Log.Error(ex, "Something went wrong while running the application");
}
finally
{
    Log.CloseAndFlush();
    Console.ReadKey();
}

