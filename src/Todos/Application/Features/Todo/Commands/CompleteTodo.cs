using BuildingBlocks.Features;
using Todos.Diagnostics;

namespace Todos.Application.Features.Todo.Commands;

public class CompleteTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app) => app.MapPut("/todos/{id}/complete",
                async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
                {
                    var command = new Command
                    {
                        TodoId = id
                    };

                    var result = await mediator.Send(command, cancellationToken);
                    return result.Match(() => Results.Ok(), ApiResults.Problem);
                })
            .WithName(nameof(CompleteTodo))
            .WithTags(nameof(Domain.Todo))
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);

    public sealed record Command : ICommand, IInvalidateCacheRequest
    {
        public Guid TodoId { get; set; }
        public string PrefixCacheKey => nameof(Domain.Todo);
    }

    internal class Handler(TodoDbContext db) :ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await db.Todos
                .SingleOrDefaultAsync(x => x.Id == request.TodoId, cancellationToken);
            
            if (todo == null)
            {
                return Result.Failure(TodoErrors.NotFound(request.TodoId));
            }

            todo.CompleteTodo();

            await db.SaveChangesAsync(cancellationToken);
            
            Instrumentation.Completed.Add(1);

            return Result.Success();
        }
    }
}
