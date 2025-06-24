using Todos.Domain;

namespace TodoUnitTests.Domain.Entities;

public class TodoTests
{
    [Fact]
    public void CreatesNewTodo()
    {
        // Act
        var todo = Todo.Create("test");

        // Assert
        todo.Title.ShouldBe("test");
        todo.Completed.ShouldBeFalse();
    }

    [Fact]
    public void ThrowsExceptionIfTitleEmpty()
    {
        // Act
        var act = () => Todo.Create("");

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowsExceptionIfTitleUpdateEmpty()
    {
        //Arrange
        var todo = Todo.Create("test");

        // Act
        var act = () => todo.UpdateTitle(string.Empty);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }
}
