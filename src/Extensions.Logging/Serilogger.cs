using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;

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

    private static LoggerConfiguration BaseConfig(this LoggerConfiguration logger)
    {
        logger
            //.Filter.ByExcluding(x => x.MessageTemplate.Text.Contains("Executing endpoint"))
            //.MinimumLevel.Information()
            //.MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            //.MinimumLevel.Override("System", LogEventLevel.Information)
            //.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            //.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .WriteTo.Async(c => c.Debug())
            .WriteTo.Async(c => c.Console());

        return logger;
    }

    private static LoggerConfiguration WriteToFile(this LoggerConfiguration logger, IConfiguration configuration, string applicationName, string environment)
    {
        var settings = configuration.GetSection("Serilog:File").Get<FileOptions>();

        if (settings == null)
        {
            return logger;
        }

        // default settings
        var defaultPath = "logs"; // default save logs files to [current directory]/logs
        var defaultTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

        // custom settings
        var configPath = settings.Path;
        var configTemplate = settings.Template;

        //var template = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
        //var template = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";
        var template = !string.IsNullOrEmpty(configTemplate) ? configTemplate : defaultTemplate;
        var path = !string.IsNullOrEmpty(configPath) ? configPath : defaultPath;

        logger.WriteTo.Async(c => c.File(@$"{path}\{applicationName}-{environment}-log-.txt",
            outputTemplate: template,
            shared: true,
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true));

        return logger;
    }

    private static LoggerConfiguration WriteToMSSQL(this LoggerConfiguration logger, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Serilog:MSSQL").Get<MSSQLOptions>();

        if (settings != null && !string.IsNullOrEmpty(settings.Connection))
        {
            var sinkOpts = new MSSqlServerSinkOptions
            {
                TableName = settings.TableName ?? "SeriLogs",
                AutoCreateSqlTable = true,
            };

            var columnOpts = new ColumnOptions
            {
                AdditionalColumns = new SqlColumn[]
                {
                    new SqlColumn
                    {
                        ColumnName = "MachineName",
                        DataType = System.Data.SqlDbType.NVarChar,
                        AllowNull = false,
                        DataLength = 100,
                    },
                    new SqlColumn
                    {
                        ColumnName = "ApplicationName",
                        DataType = System.Data.SqlDbType.NVarChar,
                        AllowNull = false,
                        DataLength = 100,
                        PropertyName = "Application"
                    }
                }
            };

            logger.WriteTo.Async(a => a.MSSqlServer(
                connectionString: settings.Connection,
                sinkOptions: sinkOpts,
                columnOptions: columnOpts,
                restrictedToMinimumLevel: LogEventLevel.Warning));
        }

        return logger;
    }

    private static LoggerConfiguration WriteToElasticsearch(this LoggerConfiguration logger, IConfiguration configuration, string applicationName, string environment)
    {
        var settings = configuration.GetSection("Serilog:Elasticsearch").Get<ElasticsearchOptions>();

        if (settings != null && !string.IsNullOrEmpty(settings.Endpoint))
        {
            var serviceName = settings.ServiceName ?? applicationName;

            logger.WriteTo.Async(w => w.Elasticsearch(new ElasticsearchSinkOptions(new Uri(settings.Endpoint))
            {
                IndexFormat = $"{serviceName}-{environment}-{DateTime.UtcNow:yyyy-MM-dd}",
                AutoRegisterTemplate = true,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                ModifyConnectionSettings = x => x.BasicAuthentication(settings.Username, settings.Password)
            }));
        }

        return logger;
    }

    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-") ?? "UnknownApp";
            var environment = context.HostingEnvironment.EnvironmentName ?? "Development";

            configuration
                .BaseConfig()
                .WriteToFile(context.Configuration, applicationName, environment)
                .WriteToMSSQL(context.Configuration)
                .WriteToElasticsearch(context.Configuration, applicationName, environment)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        };
}