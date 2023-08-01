using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Zord.Extensions.Caching;

public static class Startup
{
    public static IServiceCollection AddZordCache(this IServiceCollection services, Action<CacheOptions> action)
    {
        var setup = new CacheOptions();
        action(setup); // bind action to setup

        if (setup.Storage == "redis")
        {
            if (string.IsNullOrEmpty(setup.RedisHost))
                throw new Exception("Redis host is not configured");

            var redisConfigurationOptions = new ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { setup.RedisHost },
            };

            if (!string.IsNullOrEmpty(setup.RedisPassword))
            {
                redisConfigurationOptions.Password = setup.RedisPassword;
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = setup.RedisHost;
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


    [Obsolete("Settings will combine to other setup")]
    private static IServiceCollection AddZordMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient<ICacheService, MemoryCacheService>();

        return services;
    }

    [Obsolete("Settings will combine to other setup")]
    private static IServiceCollection AddZordDistributedCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddTransient<ICacheService, DistributedCacheService>();

        return services;
    }

    [Obsolete("Settings will combine to other setup")]
    private static IServiceCollection AddZordRedisCache(this IServiceCollection services, Action<CacheOptions> setupAction)
    {
        var setup = new CacheOptions();
        setupAction(setup);

        var redisConfigurationOptions = new ConfigurationOptions()
        {
            AbortOnConnectFail = true,
            EndPoints = { setup.RedisHost },
            ConnectRetry = 10,
            ReconnectRetryPolicy = new ExponentialRetry(5000),
        };

        if (string.IsNullOrEmpty(setup.RedisPassword) is false)
        {
            redisConfigurationOptions.Password = setup.RedisPassword;
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = setup.RedisHost;
            options.ConfigurationOptions = redisConfigurationOptions;
        });

        services.AddTransient<ICacheService, DistributedCacheService>();

        return services;
    }
}