namespace GrpcTools.GRpcDashboard.Pages
{
    using System.Collections.Generic;
    using Grpc.Net.Client;
    using GRpcTest.GRpcTodo;
    using GrpcTools.GRpcDashboard.Services;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, WeatherService weatherService, TodoService todoService)
        {
            _logger = logger;
            Weather = weatherService;
            Todo = todoService;
        }

        public IConfiguration Configuration { get; set; }

        public int TemperatureCelcius { get; private set; }

        public TodoService Todo { get; set; }

        public IEnumerable<TodoMessage> Todos { get; private set; } = new List<TodoMessage>();

        public WeatherService Weather { get; set; }

        public void OnGet()
        {
            TemperatureCelcius = Weather.GetTemperature();
            Todos = Todo.GetTodos();
        }
    }
}
