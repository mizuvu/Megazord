using Microsoft.Extensions.Hosting;

namespace Zord.Extensions.EventBus.RabbitMQ
{
    public abstract class BackgroundMessageQueueHandler<T> : BackgroundService, IMessageQueueHandler<T>
        where T : IntegrationEvent, IMessageQueue
    {
        private readonly IEventBus _event;

        protected BackgroundMessageQueueHandler(IEventBus @event)
        {
            _event = @event;
        }

        public void Consume(Action<T> handler)
        {
            var instance = Activator.CreateInstance<T>();

            _event.Consume(instance.QueueName, handler);
        }
    }
}
