using Todos.Domain;

namespace FunctionalTests.Seedwork.Builders;

internal class TodoBuilder
{
    private string _title = "todo-title";

    public TodoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public Todo Build() => Todo.Create(_title);
}
