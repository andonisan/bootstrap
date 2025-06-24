using Todos.Application.Features.Todo.Queries;

namespace FunctionalTests.Features.Todo.Queries;

public class GetTodoTestsShould(ApiServiceFixture fixture) : ApiTestBase(fixture)
{
    [Fact]
    public async Task responseOkWhenIdIsValidOne()
    {
        // Arrange
        var todo = await Given.CreateDefaultTodo();

        // Act

        var response = await Given.Client.GetAsync(ApiDefinition.V1.Todo.GetTodo(todo.Id));

        // Assert
        response.StatusCode
            .ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsJsonAsync<GetTodo.Response>();

        content.ShouldNotBeNull();

        content!.Id.ShouldBe(todo.Id);
        content!.Title.ShouldBe(todo.Title);
        content!.IsCompleted.ShouldBe(todo.Completed);
    }

    [Fact]
    public async Task responseNotFoundWhenIdIsNotExistingOne()
    {
        // Arrange
        // Act
        var response = await Given.Client.GetAsync(ApiDefinition.V1.Todo.GetTodo(Guid.NewGuid()));

        // Assert
        response.StatusCode
            .ShouldBe(HttpStatusCode.NotFound);
    }
}
