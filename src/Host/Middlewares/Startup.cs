using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Zord.Host.Middlewares;

public static class Startup
{
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
}
