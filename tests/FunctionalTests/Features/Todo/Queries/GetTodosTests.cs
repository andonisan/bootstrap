using System.Net.Http.Json;
using Todos.Application.Features.Todo.Queries;

namespace FunctionalTests.Features.Todo.Queries;

public class GetTodosTestsShould(ApiServiceFixture fixture) : ApiTestBase(fixture)
{
    [Fact]
    public async Task ResponseOk()
    {
        // Arrange
        await Given.CreateDefaultTodo();

        // Act

        var response = await Given.Client.GetFromJsonAsync<GetTodos.Response[]>(ApiDefinition.V1.Todo.GetTodos());

        // Assert
        response!.Length.ShouldBe(1);

    }
}
