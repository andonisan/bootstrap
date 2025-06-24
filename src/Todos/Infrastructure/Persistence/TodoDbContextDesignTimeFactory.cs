using Microsoft.EntityFrameworkCore.Design;

namespace Todos.Infrastructure.Persistence;

internal sealed class TodoDbContextDesignTimeFactory
    : IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        // just used it to add migrations on this project because is not 
        // a executable, is not used on runtime

        var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
        return new TodoDbContext(optionsBuilder.Options);
    }
}
