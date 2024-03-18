using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zord.Host.Middlewares;

public static class Startup
{
    private const string inboundLoggingSection = "InboundLogging";

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddInboundLogging(this IServiceCollection services)
    {
        services.AddOptions<InboundLoggingOptions>().BindConfiguration(inboundLoggingSection);

        return services;
    }

    public static IApplicationBuilder UseInboundLoggingMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(inboundLoggingSection).Get<InboundLoggingOptions>();
        if (settings is not null && settings.Enable)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRequestLoggingMiddleware(configuration);

        app.UseExceptionHandlerMiddleware();

        return app;
    }

    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        var enableRequestLogging = configuration.GetValue<bool>("RequestLogging");
        if (enableRequestLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
