using Microsoft.Extensions.Hosting;

namespace Zord.Extensions.EventBus.RabbitMQ
{
    public abstract class BackgroundEventHandler<T> : BackgroundService, IEventHandler<T>
        where T : IntegrationEvent
    {
        private readonly IEventBus _event;

        protected BackgroundEventHandler(IEventBus @event)
        {
            _event = @event;
        }

        public void Consume(string queue, Action<T> handler)
        {
            _event.Consume(queue, handler);
        }
    }
}
