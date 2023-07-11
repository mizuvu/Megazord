namespace Extensions.EventBus.Redis
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string topic, T data) where T : class;
    }
}
