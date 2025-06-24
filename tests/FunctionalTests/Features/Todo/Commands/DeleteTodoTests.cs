using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Features.Todo.Commands;

public class DeleteTodoTestsShould(ApiServiceFixture serverFixture) : ApiTestBase(serverFixture)
{
    [Fact]
    public async Task responseOkWhenDeleteOneTodo()
    {
        // Arrange
        var todo = await Given.CreateDefaultTodo();

        // Act
        var response = await Given.Client.DeleteAsync(ApiDefinition.V1.Todo.DeleteTodo(todo.Id));

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await Given.ExecuteDbContextAsync(async context =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.ShouldBeNull();
        });
    }
}
