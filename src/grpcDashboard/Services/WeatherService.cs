namespace GrpcTools.GRpcDashboard.Services
{
    using Grpc.Net.Client;
    using GRpcTest.GRpcWeather;
    using Microsoft.Extensions.Configuration;

    public class WeatherService
    {
        public WeatherService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public int GetTemperature()
        {
            var weatherChannel = GrpcChannel.ForAddress(Configuration.GetSection("Services").GetValue<string>("Weather"));
            var weather = new Weather.WeatherClient(weatherChannel);
            var temp = weather.Temperature(new LocationMessage { Name = "Bob" });
            return temp.Celsius;
        }
    }
}
