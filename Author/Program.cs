﻿using Serilog;
using TemplateCreator.Services.Config;
using TemplateCreator.Services.Csv;
using TemplateCreator.Services.Output;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit:5)
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
    Console.WriteLine("Press any key to exit.");
    Log.CloseAndFlush();
    Console.ReadKey();
}

