using System.Text.Json;
using Broadcast.Core.Entities;
using Broadcast.Infra.RabbitMQ;
using Broadcast.Infra.RabbitMQ.Events;

namespace Broadcast.Sample.Api
{
    public class WeatherForecastDeadLetterSubscriber : SubscriberEvent<Message>
    {
        public WeatherForecastDeadLetterSubscriber(IBroadcastRabbitMQ broadcast, IServiceProvider serviceProvider, string exchange = "sample", string queue = "WeatherForecast:Deadletter") : base(broadcast, serviceProvider, exchange, queue)
        {
        }

        public override async Task ProcessEvent(string request)
        {
            var content = JsonSerializer.Deserialize<Message>(request);
            await Task.FromResult(content);
        }
    }
}