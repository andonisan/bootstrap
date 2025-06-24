using Microsoft.EntityFrameworkCore;
using Todos.Application.Features.Todo.Commands;

namespace FunctionalTests.Features.Todo.Commands;

public class UpdateTodoTestsShould(ApiServiceFixture given) : ApiTestBase(given)
{
    [Fact]
    public async Task responseOkWhenUpdateOneTodo()
    {
        // Arrange
        var todo = await Given.CreateDefaultTodo();

        var command = new UpdateTodo.Command
        {
            Title = "new title"
        };

        // Act
        var response = await Given.Client.PutAsync(ApiDefinition.V1.Todo.UpdateTodo(todo.Id), command);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await Given.ExecuteDbContextAsync(async context =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.ShouldNotBeNull();
            dbTodo!.Title.ShouldBe(command.Title);
            dbTodo.Completed.ShouldBe(false);
        });
    }

    [Fact]
    public async Task ResponseBadRequestWhenTitleInvalid()
    {
        // Arrange
        var command = new UpdateTodo.Command
        {
            Title = string.Empty
        };

        // Act
        var response = await Given.Client.PutAsync(ApiDefinition.V1.Todo.UpdateTodo(Guid.Empty), command);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}
