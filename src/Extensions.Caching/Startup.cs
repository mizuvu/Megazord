using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Zord.Extensions.Caching;

public static class Startup
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