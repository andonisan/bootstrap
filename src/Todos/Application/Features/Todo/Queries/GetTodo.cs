using System.Diagnostics.CodeAnalysis;
using BuildingBlocks.Features;

namespace Todos.Application.Features.Todo.Queries;

public class GetTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/todos/{id}",
                async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(id);

                    var result = await mediator.Send(request, cancellationToken);
                        
                    return result.Match(Results.Ok, ApiResults.Problem);

                })
            .WithName(nameof(GetTodo))
            .WithTags(nameof(Domain.Todo))
            .Produces<Response>()
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);

    public sealed record Query(Guid TodoId) : IQuery<Response>;

 
    public sealed record Response(Guid Id, string Title, bool IsCompleted);

    internal sealed class Handler(ReadOnlyTodoDbContext db) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await db.Todos
                .Where(t => t.Id == request.TodoId)
                .Select(td => new Response(td.Id, td.Title, td.Completed))
                .SingleOrDefaultAsync(cancellationToken);

            return result is not null ? Result.Success(result) : Result.Failure<Response>(TodoErrors.NotFound(request.TodoId));
        }
    }

    public class RequestValidator : AbstractValidator<Query>
    {
        public RequestValidator() => RuleFor(r => r.TodoId).NotEmpty();
    }
}
