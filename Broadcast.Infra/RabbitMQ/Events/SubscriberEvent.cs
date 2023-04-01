using Microsoft.Extensions.Hosting;
using Broadcast.Core.Entities;

namespace Broadcast.Infra.RabbitMQ.Events
{
    public abstract class SubscriberEvent<T> : BackgroundService where T : Message
    {
        protected readonly IBroadcastRabbitMQ _broadcast;
        protected readonly IServiceProvider _serviceProvider;
        private readonly string _exchange;
        private readonly string _queue;

        protected SubscriberEvent(IBroadcastRabbitMQ broadcast, IServiceProvider serviceProvider, string exchange, string queue)
        {
            _broadcast = broadcast;
            _serviceProvider = serviceProvider;
            _exchange = exchange;
            _queue = queue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _broadcast.SubscribeAsync<T>(_exchange, _queue, async request => await ProcessEvent(request));

            return Task.CompletedTask;
        }

        public abstract Task ProcessEvent(string request);
    }
}