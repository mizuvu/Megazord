using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Extensions.EventBus.Redis
{
    public static class Startup
    {
        public static void AddEventBusRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["CacheSettings:RedisURL"];

            ArgumentNullException.ThrowIfNull(url);

            var password = configuration["CacheSettings:RedisPassword"];

            var redisConfigurationOptions = new ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { url },
                Password = password
            };

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfigurationOptions));

            services.AddSingleton<IRedisEventBus, RedisEventBus>();
        }
    }
}
