namespace Zord.Extensions.EventBus.RabbitMQ
{
    public interface IEventHandler<T>
         where T : IntegrationEvent
    {
        void Consume(string queue, Action<T> handler);
    }
}