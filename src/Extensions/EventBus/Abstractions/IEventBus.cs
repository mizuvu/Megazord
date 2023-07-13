using System;
using Zord.Extensions.EventBus.Events;

namespace Zord.Extensions.EventBus.Abstractions
{
    public interface IEventBus : IDisposable
    {
        void Publish<T>(string queueName, T message)
            where T : IntegrationEvent;

        void Publish<T>(T messageQueue)
             where T : IntegrationEvent, IMessageQueue;

        void Consume<T>(string queueName, Action<T> handler)
            where T : IntegrationEvent;
    }
}
