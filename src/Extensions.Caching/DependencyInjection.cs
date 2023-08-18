using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Zord.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddZordCache(this IServiceCollection services, Action<CacheOptions> action)
    {
        var setup = new CacheOptions();
        action(setup); // bind action to setup

        services.AddZordCache(setup);

        return services;
    }

    public static IServiceCollection AddZordCache(this IServiceCollection services, CacheOptions settings)
    {
        ArgumentNullException.ThrowIfNull(settings, nameof(CacheOptions));

        if (settings.Provider == "redis")
        {
            if (string.IsNullOrEmpty(settings.RedisHost))
                throw new Exception("Redis host is not configured");

            var redisConfigurationOptions = new ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { settings.RedisHost },
            };

            if (!string.IsNullOrEmpty(settings.RedisPassword))
            {
                redisConfigurationOptions.Password = settings.RedisPassword;
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = settings.RedisHost;
                options.ConfigurationOptions = redisConfigurationOptions;
            });

            services.AddTransient<ICacheService, DistributedCacheService>();

            return services;
        }

        // default will user memory cache
        services.AddMemoryCache();
        services.AddTransient<ICacheService, MemoryCacheService>();

        return services;
    }

    private static IServiceCollection AddZordDistributedCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddTransient<ICacheService, DistributedCacheService>();

        return services;
    }
}