using Contracts;

namespace Todos.Domain;

internal class Todo : BaseEntity
{
    // This protected constructor is required for EF Core
    protected Todo()
    {
    }

    public string Title { get; private set; } = null!;

    public bool Completed { get; private set; }

    private Todo(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Raise(new TodoCreatedEvent(Id));
        Title = title;
    }
    public static Todo Create(string title) => new(title);

    public void UpdateTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
    }

    public void CompleteTodo()
    {
        if (Completed)
        {
            return;
        }

        Completed = true;
        Raise(new TodoCompletedEvent(Id, Title));
    }
}
