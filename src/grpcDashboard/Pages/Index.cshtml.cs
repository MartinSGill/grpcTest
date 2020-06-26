namespace GrpcTools.GRpcDashboard.Pages
{
    using System.Collections.Generic;
    using Grpc.Core;
    using Grpc.Net.Client;
    using GRpcTest.GRpcTodo;
    using GRpcTest.GRpcWeather;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public int TemperatureCelcius { get; private set; } = 0;

        public IEnumerable<TodoMessage> Todos { get; private set; } = new List<TodoMessage>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var weatherChannel = GrpcChannel.ForAddress("https://localhost:5002");
            var weather = new Weather.WeatherClient(weatherChannel);
            var temp = weather.Temperature(new LocationMessage { Name = "Bob" });
            TemperatureCelcius = temp.Celsius;

            var todoChannel = GrpcChannel.ForAddress("https://localhost:5003");
            var todoClient = new Todo.TodoClient(todoChannel);
            var todos = todoClient.GetTodos(new GetTodosMessage());
            Todos = todos.Todos;
        }
    }
}
