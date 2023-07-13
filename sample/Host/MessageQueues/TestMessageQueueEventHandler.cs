using Zord.Extensions.EventBus.Abstractions;
using Zord.Extensions.EventBus.RabbitMQ;

namespace Host.MessageQueues;

public class TestMessageQueueEventHandler
    : BackgroundMessageQueueHandler<TestMessageQueue>
{
    public TestMessageQueueEventHandler(IEventBus @event) : base(@event)
    {
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Consume(mess =>
        {
            Console.WriteLine($"{mess.QueueName}");

            Console.WriteLine($"--------");

            foreach (var t in mess.Data)
            {
                Console.WriteLine($"{t}");
            }
        });

        return Task.CompletedTask;
    }
}
