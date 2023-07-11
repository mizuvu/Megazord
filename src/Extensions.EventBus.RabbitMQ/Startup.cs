global using Zord.Extensions.EventBus.Abstractions;
global using Zord.Extensions.EventBus.Events;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Zord.Extensions.EventBus.RabbitMQ;

public static class Startup
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQOptions> action)
    {
        // get values from Action<T>
        var opt = new RabbitMQOptions();
        action(opt);

        var retryConnectTimes = opt.RetryConnectTimes;

        // Retry connect to connect RabbitMQ 100 times if fail
        // Fix when Windows server restart, IIS site error because RabbitMQ service start after IIS Sites
        for (int i = 0; i < retryConnectTimes; i++)
        {
            try
            {
                //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
                var factory = new ConnectionFactory
                {
                    HostName = opt.HostName,
                    UserName = opt.UserName,
                    Password = opt.Password,

                    // automatic retry connect RabbitMQ
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5), // retry connect every 5s if connect failed
                };

                //Create the RabbitMQ connection using connection factory details as i mentioned above
                IConnection connection = factory.CreateConnection();
                services.AddSingleton(connection);

                services.AddSingleton<IEventBus, RabbitMQEventBus>();

                Console.WriteLine("----- RabbitMQ connected");

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine("----- RabbitMQ connect error: " + ex.Message);

                Thread.Sleep(5000);
            }
        }

        return services;
    }

    public static void Subscribe<TMessage, TEventHandler>(this IServiceCollection services)
        where TMessage : IntegrationEvent
        where TEventHandler : class, IEventHandler<TMessage>, IHostedService
    {
        services.AddHostedService<TEventHandler>();
    }
}