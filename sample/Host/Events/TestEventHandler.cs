using Zord.Extensions.EventBus.Abstractions;
using Zord.Extensions.EventBus.Events;
using Zord.Extensions.EventBus.RabbitMQ;

namespace Host.Events;

public class TestEventHandler
    : BackgroundEventHandler<MessageQueue<TestEvent>>
{
    public TestEventHandler(IEventBus @event) : base(@event)
    {
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Consume("TestQueue", mess =>
        {
            Console.WriteLine($"{mess.Message.Id} - {mess.Message.Name}");
        });

        return Task.CompletedTask;
    }
}
