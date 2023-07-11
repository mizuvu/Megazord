using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Zord.Extensions.EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly ILogger<RabbitMQEventBus> _logger;

        public RabbitMQEventBus(IConnection connection,
            ILogger<RabbitMQEventBus> logger)
        {
            _connection = connection;
            _model = _connection.CreateModel();
            _logger = logger;
        }

        public void Publish<T>(string queue, T message)
             where T : IntegrationEvent
        {
            //declare the queue after mentioning name and a few property related to that
            _model.QueueDeclare(queue, exclusive: false);

            //Serialize the message
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            //put the data on to the product queue
            _model.BasicPublish(exchange: "", routingKey: queue, body: body);

            _logger.LogInformation("----- [RabbitMQ] Queue {queue}: {id} published", queue, message.Id);
        }

        public void Consume<T>(string queue, Action<T> handler)
            where T : IntegrationEvent
        {
            //declare the queue after mentioning name and a few property related to that
            _model.QueueDeclare(queue, exclusive: false);

            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var obj = JsonSerializer.Deserialize<T>(message);

                ArgumentNullException.ThrowIfNull(obj);

                handler(obj);

                _logger.LogInformation("----- [RabbitMQ] Queue {queue}: {id} consumed", queue, obj.Id);
            };
            //read the message
            _model.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();

            if (_connection.IsOpen)
                _connection.Close();

            GC.SuppressFinalize(this);
        }
    }
}
