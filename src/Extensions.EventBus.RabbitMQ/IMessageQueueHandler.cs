namespace Zord.Extensions.EventBus.RabbitMQ
{
    public interface IMessageQueueHandler<T>
         where T : IntegrationEvent, IMessageQueue
    {
        void Consume(Action<T> handler);
    }
}