using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Zord.Extensions.Logging;

public static class Serilogger
{
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Serilog.Core.Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }

    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            // default settings
            var defaultPath = "logs";
            var defaultTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

            // custom settings
            var configPath = context.Configuration.GetValue<string>("Serilog:FilePath");
            var configTemplate = context.Configuration.GetValue<string>("Serilog:Template");

            var template = !string.IsNullOrEmpty(configTemplate) ? configTemplate : defaultTemplate;
            var path = !string.IsNullOrEmpty(configPath) ? configPath : defaultPath;

            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-") ?? "UnknownApp";
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

            //var template = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
            //var template = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";

            configuration
                //.Filter.ByExcluding(x => x.MessageTemplate.Text.Contains("Executing endpoint"))
                //.MinimumLevel.Information()
                //.MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                //.MinimumLevel.Override("System", LogEventLevel.Information)
                //.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                //.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .WriteTo.Async(c => c.Debug())
                .WriteTo.Async(c => c.Console())
                .WriteTo.Async(c => c.File(@$"{path}\{applicationName}-{environmentName}-log-.txt",
                    outputTemplate: template,
                    shared: true,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true))
                .WriteTo.Async(c => c.File(@$"{path}\{applicationName}-{environmentName}-critical-.txt",
                    outputTemplate: template,
                    shared: true,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    restrictedToMinimumLevel: LogEventLevel.Warning))
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        };
}