using Microsoft.AspNetCore.Mvc;
using Broadcast.Infra.RabbitMQ;

namespace Broadcast.Sample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBroadcastRabbitMQ _bus;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IBroadcastRabbitMQ bus)
    {
        _logger = logger;
        _bus = bus;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var list = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        foreach (var item in list)
        {
            await _bus.PublishAsync("sample", nameof(WeatherForecast), item);
        }

        return list;
    }
}
