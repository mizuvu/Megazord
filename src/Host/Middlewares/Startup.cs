using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zord.Host.Middlewares;

public static class Startup
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
        var enableRequestLogging = configuration.GetValue<bool>("RequestLogging");
        if (enableRequestLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        app.UseMiddleware<ExceptionHandlerMiddleware>();

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

    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
