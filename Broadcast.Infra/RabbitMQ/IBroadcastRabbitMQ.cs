using Broadcast.Core.Entities;
using RabbitMQ.Client;

namespace Broadcast.Infra.RabbitMQ
{
    public interface IBroadcastRabbitMQ
    {
        void TryConnect(IConnectionFactory connectionFactory);
        Task PublishAsync(string exchange, string queue, Message message);
        Task SubscribeAsync<T>(string exchange, string queue, Func<string, Task> action);
    }
}