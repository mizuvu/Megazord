using Microsoft.Extensions.DependencyInjection;

namespace Zord.Extensions.Logging
{
    public static class ConfigureService
    {
        public static IServiceCollection AddZordLogging(this IServiceCollection services) =>
            services.AddScoped<ILogger, Logger>();
    }
}
