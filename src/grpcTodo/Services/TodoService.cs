namespace GRpcTest.GRpcTodo.Services
{
    using System;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;

    public class TodoService : Todo.TodoBase
    {
        private readonly ILogger<TodoService> _logger;

        public TodoService(ILogger<TodoService> logger)
        {
            _logger = logger;
        }

        public override Task<GetTodosReply> GetTodos(GetTodosMessage request, ServerCallContext context)
        {
            _logger.LogInformation("GetTodos Called");
            var result = new GetTodosReply();
            result.Todos.AddRange(
                new[]
                {
                    new TodoMessage
                    {
                        Date = DateTime.Now.ToUniversalTime().ToShortDateString(),
                        Priority = 1,
                        Description = "This is a todo"
                    },
                    new TodoMessage
                    {
                        Date = DateTime.Now.ToUniversalTime().ToShortDateString(),
                        Priority = 2,
                        Description = "This is another todo"
                    }
                });

            return Task.FromResult(result);
        }
    }
}
