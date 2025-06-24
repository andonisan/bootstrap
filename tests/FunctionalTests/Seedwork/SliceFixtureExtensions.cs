using FunctionalTests.Seedwork.Builders;
using Microsoft.Extensions.DependencyInjection;
using Todos.Domain;
using Todos.Infrastructure.Persistence;

namespace FunctionalTests.Seedwork;

internal static class SliceFixtureExtensions
{
    public static async Task InsertAsync<T>(this ApiServiceFixture fixture, T entity) where T : class
    {
        using var scope = fixture.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task InsertAsync<T>(this ApiServiceFixture fixture, params T[] entities) where T : class
    {
        using var scope = fixture.Services.CreateScope();
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

            foreach (var entity in entities)
            {
                dbContext.Set<T>().Add(entity);
            }

            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task<Todo> CreateDefaultTodo(this ApiServiceFixture serverFixture, CancellationToken cancellationToken = default)
    {
        var todo = TestBuilders.GetTodosBuilder()
            .Build();

        await serverFixture.InsertAsync(todo);

        return todo;
    }
}
