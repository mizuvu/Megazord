using System;
using Zord.Extensions.EventBus.Events;

namespace Zord.Extensions.EventBus.Abstractions
{
    public interface IEventBus : IDisposable
    {
        void Publish<T>(string queue, T message)
            where T : IntegrationEvent;

        void Consume<T>(string queue, Action<T> handler)
            where T : IntegrationEvent;
    }
}
