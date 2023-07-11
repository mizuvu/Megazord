namespace Zord.Extensions.EventBus.Events
{
    public class MessageQueue<T> : IntegrationEvent
    {
        public MessageQueue(T message)
        {
            Message = message;
        }

        public T Message { get; set; }
    }
}