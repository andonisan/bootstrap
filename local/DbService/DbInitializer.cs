using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Todos.Domain;
using Todos.Infrastructure.Persistence;

namespace DbService;

internal sealed class DbInitializer(IServiceProvider serviceProvider, ILogger<DbInitializer> logger)
    : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        using var activity = _activitySource.StartActivity("Initializing database", ActivityKind.Client);
        await InitializeDatabaseAsync(dbContext, cancellationToken);
    }

    public async Task InitializeDatabaseAsync(TodoDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

        await SeedAsync(dbContext, cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms",
            sw.ElapsedMilliseconds);
    }

    private async Task SeedAsync(TodoDbContext dbContext, CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding database");

        if (!await dbContext.Todos.AnyAsync(cancellationToken))
        {
            dbContext.Todos.AddRange(
                Todo.Create("Ines"),
                Todo.Create("Luisa"),
                Todo.Create("Martina")
            );

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
    
    public override void Dispose()
    {
        _activitySource.Dispose();
        base.Dispose();
    }
}
