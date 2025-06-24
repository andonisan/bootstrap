using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Features.Todo.Commands;

public class CompleteTodoTestsShould(ApiServiceFixture given) : ApiTestBase(given)
{
    [Fact]
    public async Task responseOkWhenCompleteOneTodo()
    {
        // Arrange
        var todo = await Given.CreateDefaultTodo();

        // Act
        var response = await Given.Client.PutAsync(ApiDefinition.V1.Todo.CompleteTodo(todo.Id),null);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await Given.ExecuteDbContextAsync(async context =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.ShouldNotBeNull();
            dbTodo!.Completed.ShouldBe(true);
        });
    }
}
