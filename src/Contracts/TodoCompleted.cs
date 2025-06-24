namespace Contracts;

public record TodoCompleted
{
    public string Title { get; init; } = null!;
    public string User { get; set; } = null!;
}
