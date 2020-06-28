namespace GrpcTools.GRpcDashboard.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using Google.Protobuf.Collections;
    using Grpc.Net.Client;
    using GRpcTest.GRpcTodo;
    using GRpcTest.GRpcWeather;
    using Microsoft.Extensions.Configuration;

    public class TodoService
    {
        public TodoService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public IEnumerable<TodoMessage> GetTodos()
        {
            var todoChannel = GrpcChannel.ForAddress(Configuration.GetSection("Services").GetValue<string>("Todo"));
            var todo = new Todo.TodoClient(todoChannel);
            return todo.GetTodos(new GetTodosMessage()).Todos;
        }
    }
}
