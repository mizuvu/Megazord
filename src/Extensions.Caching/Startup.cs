using Microsoft.Extensions.DependencyInjection;

namespace Zord.Extensions.Caching;

public static class Startup
{
    public static IServiceCollection AddZordMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient<ICacheService, LocalCacheService>();

        return services;
    }

    public static IServiceCollection AddZordDistributedCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddTransient<ICacheService, DistributedCacheService>();

        return services;
    }

    public static IServiceCollection AddZordRedisCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
    {
        var setup = new RedisCacheOptions();
        setupAction(setup);

        var redisConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
        {
            AbortOnConnectFail = true,
            EndPoints = { setup.RedisURL }
        };

        if (string.IsNullOrEmpty(setup.RedisPassword) is false)
        {
            redisConfigurationOptions.Password = setup.RedisPassword;
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = setup.RedisURL;
            options.ConfigurationOptions = redisConfigurationOptions;
        });

        services.AddTransient<ICacheService, DistributedCacheService>();

        return services;
    }
}