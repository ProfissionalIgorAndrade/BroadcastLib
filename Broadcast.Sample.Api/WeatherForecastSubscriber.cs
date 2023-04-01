using System.Text.Json;
using Broadcast.Infra.RabbitMQ;
using Broadcast.Infra.RabbitMQ.Events;

namespace Broadcast.Sample.Api
{
    public class WeatherForecastSubscriber : SubscriberEvent<WeatherForecast>
    {
        public WeatherForecastSubscriber(IBroadcastRabbitMQ broadcast, IServiceProvider serviceProvider, string exchange = "sample", string queue = nameof(WeatherForecast)) : base(broadcast, serviceProvider, exchange, queue)
        {
        }

        public override async Task ProcessEvent(string request)
        {
            var content = JsonSerializer.Deserialize<WeatherForecast>(request);
            await Task.FromResult(content);
        }
    }
}