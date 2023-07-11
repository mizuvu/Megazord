namespace Extensions.EventBus.Redis
{
    public interface IEventSubscriber
    {
        Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class;
    }
}
