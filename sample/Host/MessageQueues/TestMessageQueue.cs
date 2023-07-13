using Zord.Extensions.EventBus.Events;

namespace Host.MessageQueues
{
    public class TestMessageQueue : IntegrationEvent, IMessageQueue
    {
        public string QueueName => "Test_MessageQueue";

        public string[] Data { get; set; } = null!;
    }
}