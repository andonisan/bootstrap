using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Todos.Application.Features.Todo.Commands;

namespace FunctionalTests.Features.Todo.Commands;

public class CreateTodoTestsShould(ApiServiceFixture fixture) : ApiTestBase(fixture)
{

    [Fact]
    public async Task responseOkWhenCreateOneTodo()
    {
        // Arrange
        var command = new CreateTodo.Command
        {
            Title = "test"
        };

        // Act
        var response = await Given.Client.PostAsJsonAsync(ApiDefinition.V1.Todo.CreateTodo(), command);

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
        var command = new CreateTodo.Command
        {
            Title = string.Empty
        };

        // Act
        var response = await Given.Client.PostAsync(ApiDefinition.V1.Todo.CreateTodo(), command);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}
