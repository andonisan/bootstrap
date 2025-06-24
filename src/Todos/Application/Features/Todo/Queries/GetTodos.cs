using BuildingBlocks.Features;

namespace Todos.Application.Features.Todo.Queries;

public class GetTodos : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/todos",
                async (ISender mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new Query(), cancellationToken);

                    return result.Match(Results.Ok, ApiResults.Problem);
                })
            .WithName(nameof(GetTodos))
            .WithTags(nameof(Domain.Todo))
            .Produces<Response[]>();

    public record Query : IQuery<Response[]>, ICacheRequest
    {
        public string CacheKey => nameof(Domain.Todo);
        public DateTime? AbsoluteExpirationRelativeToNow { get; }
    }

    public record Response(Guid Id, string Title, bool IsCompleted);

    internal sealed class Handler(
        ReadOnlyTodoDbContext db,
        Diagnostics.Diagnostics diagnostics)
        : IQueryHandler<Query, Response[]>
    {
        public async Task<Result<Response[]>> Handle(Query request, CancellationToken cancellationToken = default)
        {
            diagnostics.GetTodoRequest();

            return await db.Todos
                .Select(t => new Response(t.Id, t.Title, t.Completed))
                .ToArrayAsync(cancellationToken);
        }
    }
}
