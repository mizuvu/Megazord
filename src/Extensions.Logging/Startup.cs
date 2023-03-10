global using Zord.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Zord.Extensions.Logging
{
    public static class Startup
    {
        public static IServiceCollection AddZordLogging(this IServiceCollection services) =>
            services.AddScoped<ILogger, Logger>();
    }
}