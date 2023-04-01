using Broadcast.Core.Entities;
using Broadcast.Core.Logics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Broadcast.Infra.RabbitMQ
{
    public class BroadcastRabbitMQ : IBroadcastRabbitMQ, IDisposable
    {
        private IModel _channel;
        private IConnection _connection;
        private IConnectionFactory _connectionFactory;

        public BroadcastRabbitMQ(IConnectionFactory connectionFactory)
        {
            TryConnect(connectionFactory);
        }

        public void TryConnect(IConnectionFactory connectionFactory)
        {
            if (_connectionFactory == null)
            {
                _connectionFactory = connectionFactory;
            }

            _connection = _connectionFactory
                  .CreateConnection();

            if (IsConnected)
            {
                Console.WriteLine($"Message Broker {IsConnected}");
            }
            else
            {
                Console.WriteLine($"Message Broker {IsConnected}");
            }
        }

        public Task PublishAsync(string exchange, string queue, Message message)
        {
            var channel = DeclareChannel(exchange, queue);

            var routingKey = CreateQueueName(exchange, queue);

            var body = ConvertMessage.ToByte(message);

            channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);

            return Task.CompletedTask;
        }

        public Task SubscribeAsync<T>(string exchange, string queue, Func<string, Task> action)
        {
            var channel = DeclareChannel(exchange, queue);

            var routingKey = CreateQueueName(exchange, queue);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                var bytes = ea.Body.ToArray();

                var body = ConvertMessage.ToJson(bytes);

                try
                {
                    await action.Invoke(body);
                }
                catch
                {
                    //TODO - Como converter dado novamente para Objeto recebido
                    await PublishAsync(exchange, CreateQueueDeadletterName(queue), new Message());
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            try
            {
                _channel.BasicConsume(routingKey, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Task.CompletedTask;
        }

        private IModel DeclareChannel(string exchange, string queue)
        {
            if (_channel == null)
            {
                _channel = _connection.CreateModel();
            }

            _channel.QueueDeclare(queue: CreateQueueName(exchange, queue), durable: true, exclusive: false, autoDelete: false, arguments: null);

            return _channel;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        private string CreateQueueName(string brokerName, string eventName) => $"{brokerName}:{eventName}";

        private string CreateQueueDeadletterName(string queue) => $"{queue}:Deadletter";

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}