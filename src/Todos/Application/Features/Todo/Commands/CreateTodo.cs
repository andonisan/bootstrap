using BuildingBlocks.Features;
using Todos.Diagnostics;

namespace Todos.Application.Features.Todo.Commands;

public class CreateTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/todos",
                async (Command command, ISender mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(command, cancellationToken);

                    return result.Match(() => Results.Ok(), ApiResults.Problem);
                })
            .WithName(nameof(CreateTodo))
            .WithTags(nameof(Domain.Todo))
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

    public sealed record Command : ICommand, IInvalidateCacheRequest
    {
        public string Title { get; set; } = string.Empty;
        public string PrefixCacheKey => nameof(Domain.Todo);
    }

    internal sealed class Handler(TodoDbContext db) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = Domain.Todo.Create(request.Title);

            await db.Todos.AddAsync(todo, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);

            Instrumentation.Created.Add(1);

            return Result.Success();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(r => r.Title).NotEmpty();
    }
}
