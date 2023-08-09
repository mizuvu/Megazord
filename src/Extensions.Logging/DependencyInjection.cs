using Microsoft.Extensions.DependencyInjection;
using Zord.Core;
using Zord.Extensions.Logging;

namespace Zord.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddZordLogging(this IServiceCollection services)
            => services.AddScoped<ILogger, Logger>();
    }
}
