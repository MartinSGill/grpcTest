namespace GRpcTest.GRpcWeather.Services
{
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;

    public class WeatherService : Weather.WeatherBase
    {
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public override Task<TemperatureReply> Temperature(LocationMessage request, ServerCallContext context)
        {
            return Task.FromResult(
                new TemperatureReply { Celsius = 100, Fahrenheit = 212, Kelvin = 373 });
        }
    }
}
