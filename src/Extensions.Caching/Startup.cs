global using Zord.Core.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zord.Extensions.Caching
{
    public static class Startup
    {
        public static IServiceCollection AddZordCaching(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection("Caching").Get<CacheConfiguration>();

            if (settings is null || !settings.UseDistributedCache)
            {
                services.AddMemoryCache();
                services.AddTransient<ICacheService, LocalCacheService>();

                Console.WriteLine("----- Use Memory cache");

                return services;
            }

            if (settings.PreferRedis)
            {
                var redisConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    EndPoints = { settings.RedisURL }
                };

                if (!string.IsNullOrEmpty(settings.RedisPassword))
                {
                    redisConfigurationOptions.Password = settings.RedisPassword;
                }

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = settings.RedisURL;
                    options.ConfigurationOptions = redisConfigurationOptions;
                });

                Console.WriteLine("----- Use Redis cache");
            }
            else
            {
                services.AddDistributedMemoryCache();

                Console.WriteLine("----- Use Distributed Memory cache");
            }

            services.AddTransient<ICacheService, DistributedCacheService>();

            return services;
        }
    }
}