using BuildingBlocks.Features;

namespace Todos.Application.Features.Todo.Commands;

public class UpdateTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/todos/{id}",
                async (Guid id, Command command, ISender mediator, CancellationToken cancellationToken) =>
                {
                    command.TodoId = id;
                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(() => Results.Ok(), ApiResults.Problem);
                })
            .WithName(nameof(UpdateTodo))
            .WithTags(nameof(Domain.Todo))
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);

    public sealed record Command : ICommand, IInvalidateCacheRequest
    {
        public Guid TodoId { get; internal set; }
        public string Title { get; set; } = null!;
        public string PrefixCacheKey => nameof(Domain.Todo);
    }

    internal sealed class Handler(TodoDbContext db) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await db.Todos
                .SingleOrDefaultAsync(x => x.Id == request.TodoId, cancellationToken);
            if (todo == null)
            {
                return Result.Failure(TodoErrors.NotFound(request.TodoId));
            }

            todo.UpdateTitle(request.Title);

            await db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(r => r.TodoId).NotEmpty();
            RuleFor(r => r.Title).NotEmpty();
        }
    }
}
