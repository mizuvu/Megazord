using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Extensions.EventBus.Redis
{
    public class RedisEventBus : IRedisEventBus
    {
        private readonly ISubscriber _subscriber;
        private readonly ILogger<RedisEventBus> _logger;

        public RedisEventBus(IConnectionMultiplexer connectionMultiplexer,
            ILogger<RedisEventBus> logger)
        {
            _subscriber = connectionMultiplexer.GetSubscriber();
            _logger = logger;
        }

        public async Task PublishAsync<T>(string topic, T data) where T : class
        {
            var json = JsonSerializer.Serialize(data);
            var value = Encoding.Default.GetBytes(json);

            await _subscriber.PublishAsync(topic, value);

            _logger.LogInformation("----- [Event] topic {topic} published", topic);
        }

        public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class
        => _subscriber.SubscribeAsync(topic, (_, data) =>
        {
            var payload = Encoding.Default.GetString(data!);
            var obj = JsonSerializer.Deserialize<T>(payload);
            if (obj is null)
            {
                return;
            }

            handler(obj);
        });
    }
}
