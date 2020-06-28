namespace GrpcTools.GRpcDashboard.Services
{
    using System;
    using GRpcTest.GRpcWeather;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class WeatherService
    {
        private readonly GrpcChannelProvider channelProvider;

        private readonly string channelUrl;
        private readonly ILogger<WeatherService> logger;

        public WeatherService(
            IConfiguration configuration,
            ILogger<WeatherService> logger,
            GrpcChannelProvider channelProvider)
        {
            this.logger = logger;
            channelUrl = configuration.GetSection("Services").GetValue<string>("Weather");
            logger.LogInformation("Channel URL {TodoUrl}", channelUrl);
            this.channelProvider = channelProvider;
        }

        public int GetTemperature()
        {
            try
            {
                var weatherChannel = channelProvider.GetChannel(channelUrl);
                var weather = new Weather.WeatherClient(weatherChannel);
                var temp = weather.Temperature(new LocationMessage { Name = "Bob" });
                return temp.Celsius;
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unable to get temperature.");
                var inner = e?.InnerException;
                while (inner != null)
                {
                    logger.LogError(inner, "Inner Exception.");
                    inner = inner?.InnerException;
                }

                throw;
            }
        }
    }
}
