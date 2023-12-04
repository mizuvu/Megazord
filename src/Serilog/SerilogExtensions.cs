﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using Serilogger = Serilog.Core.Logger;

namespace Zord.Serilog
{
    public static class SerilogExtensions
    {
        public static void EnsureInitialized()
        {
            if (Log.Logger.GetType() != typeof(Serilogger))
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
            var elementName = "FileAsync";

            var options = SerilogOptionsExtensions.GetWriteToOptions(configuration, elementName);

            if (options == null)
            {
                return logger;
            }

            // custom settings
            string? configPath = null;
            string? configTemplate = null;

            // get options values
            if (options.Args != null)
            {
                options.Args.TryGetValue("Path", out configPath);
                options.Args.TryGetValue("Template", out configTemplate);
            }

            // default settings
            var defaultPath = "logs"; // default save logs files to [current directory]/logs
            var defaultTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

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
        /*
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
                    new SqlColumn() {
                        ColumnName = "MachineName",
                        DataType = System.Data.SqlDbType.NVarChar,
                        AllowNull = false,
                        DataLength = 100,
                    },
                    new SqlColumn() {
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
        */
        private static LoggerConfiguration WriteToElasticsearch(this LoggerConfiguration logger, IConfiguration configuration, string applicationName, string environment)
        {
            var elementName = "ElasticsearchAsync";

            var options = SerilogOptionsExtensions.GetWriteToOptions(configuration, elementName);

            if (options != null && options.Args != null)
            {
                // get Elasticsearch configuration in WriteTo Args
                options.Args.TryGetValue("ServiceName", out string? serviceName);
                options.Args.TryGetValue("Endpoint", out string? endpoint);
                options.Args.TryGetValue("Username", out string? username);
                options.Args.TryGetValue("Password", out string? password);

                if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    if (string.IsNullOrEmpty(serviceName))
                    {
                        serviceName = applicationName;
                    }

                    logger.WriteTo.Async(w => w.Elasticsearch(new ElasticsearchSinkOptions(new Uri(endpoint))
                    {
                        IndexFormat = $"{serviceName}-{environment}-{DateTime.UtcNow:yyyy-MM-dd}",
                        AutoRegisterTemplate = true,
                        NumberOfReplicas = 1,
                        NumberOfShards = 2,
                        ModifyConnectionSettings = x => x.BasicAuthentication(username, password)
                    }));
                }
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
                    //.WriteToMSSQL(context.Configuration)
                    .WriteToElasticsearch(context.Configuration, applicationName, environment)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Environment", environment)
                    .Enrich.WithProperty("Application", applicationName)
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}