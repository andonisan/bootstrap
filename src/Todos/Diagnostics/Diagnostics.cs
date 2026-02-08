using Microsoft.Extensions.Logging;
using Todos.Application.Features.Todo.Commands;

namespace Todos.Diagnostics;

internal partial class Diagnostics
{
    private readonly ILogger _logger;

    public Diagnostics(ILoggerFactory loggerFactory)
    {
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger("TodoApp.App");
    }

    [LoggerMessage(EventId = 101,
        EventName = nameof(GetTodoRequest),
        Level = LogLevel.Information,
        Message = "Starting request for Todo's.")]
    public partial void GetTodoRequest();

    [LoggerMessage(EventId = 100,
        EventName = nameof(GetTodoRequestThrow),
        Level = LogLevel.Error,
        Message = "Get request for Todo's throw an exception.")]
    public partial void GetTodoRequestThrow(Exception exception);

    [LoggerMessage(
        EventName = nameof(ErrorCreatingTodo),
        Level = LogLevel.Error,
        Message = "Error creating Todo in {command}")]
    public partial void ErrorCreatingTodo(CreateTodo.Command command);
}
