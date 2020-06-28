namespace GrpcTools.GRpcDashboard.Services
{
    using System;
    using System.Collections.Generic;
    using GRpcTest.GRpcTodo;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class TodoService
    {
        private readonly GrpcChannelProvider channelProvider;

        private readonly string channelUrl;
        private readonly ILogger<TodoService> logger;

        public TodoService(
            IConfiguration configuration,
            ILogger<TodoService> logger,
            GrpcChannelProvider channelProvider)
        {
            this.logger = logger;
            channelUrl = configuration.GetSection("Services").GetValue<string>("Todo");
            logger.LogInformation("Channel URL {TodoUrl}", channelUrl);
            this.channelProvider = channelProvider;
        }

        public IEnumerable<TodoMessage> GetTodos()
        {
            try
            {
                var todoChannel = channelProvider.GetChannel(channelUrl);
                var todo = new Todo.TodoClient(todoChannel);
                return todo.GetTodos(new GetTodosMessage()).Todos;
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unable to get todos.");
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
