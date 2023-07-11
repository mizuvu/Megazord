using Microsoft.Extensions.DependencyInjection;

namespace Zord.EntityFrameworkCore.Cache;

public static class ConfigureService
{
    public static IServiceCollection AddZordDynamicCacheRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICacheRepository<,>), typeof(CacheRepository<,>));

        return services;
    }
}