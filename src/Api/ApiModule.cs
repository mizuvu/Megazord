using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zord.Api.Extensions;

namespace Zord.Api;

public static class ApiModule
{
    /// <summary>
    /// add API version
    /// </summary>
    /// <param name="services"></param>
    /// <param name="version"></param>
    /// <param name="minorVersion"></param>
    /// <returns></returns>
    public static IApiVersioningBuilder AddApiVersion(this IServiceCollection services, int version, int minorVersion = 0)
        => services
        .AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(version, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        })
        .AddApiExplorer(o =>
        {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });

    /// <summary>
    /// use configuration files *.json from folder
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder host)
    {
        var env = host.Environment.EnvironmentName;
        var configuration = host.Configuration;

        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

        // load *.json file from folder Configurations
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Configurations");
        var dInfo = new DirectoryInfo(path);
        var files = dInfo.GetFiles("*.json").Where(x => !x.Name.Contains("appsettings"));

        foreach (var file in files)
        {
            configuration
                .AddJsonFile(Path.Combine(path, file.Name), optional: false, reloadOnChange: true);
        }

        configuration.AddEnvironmentVariables();

        return host;
    }

    /// <summary>
    /// Custom controller options for web api
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerNameConvention());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // custom Invalid model state response
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key]!.Errors.Select(x => $"{key}: {x.ErrorMessage}"))
                    .ToArray();

                var apiError = Result.Result.BadRequest("", errors);

                var result = new ObjectResult(apiError)
                {
                    StatusCode = 400
                };
                result.ContentTypes.Add(MediaTypeNames.Application.Json);

                return result;
            };
        });

        return services;
    }
}