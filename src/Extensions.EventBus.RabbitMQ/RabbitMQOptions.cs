namespace Zord.Extensions.EventBus.RabbitMQ
{
    public class RabbitMQOptions
    {
        public string? HostName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int RetryConnectTimes { get; set; } = 50;
    }
}
