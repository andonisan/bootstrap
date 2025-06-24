namespace Todos.Application.Features.Todo;

public static class TodoErrors
{
    public static Error NotFound(Guid todoId) =>
        Error.NotFound(
            "Todos.NotFound",
            $"The todo with identifier {todoId} was not found");
}
