using DbService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ServiceDefaults;
using Todos.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("TodoAppDb");

    options.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(typeof(TodoDbContext).Assembly));

});

builder.Services.AddProblemDetails();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<DbInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<DbInitializerHealthCheck>(nameof(DbInitializer));

var app = builder.Build();

app.UseExceptionHandler();

app.MapPost("/reset-db", async (TodoDbContext db, DbInitializer dbInitializer, CancellationToken cancellationToken) =>
        // Delete and recreate the database. This is useful for development scenarios to reset the database to its initial state.
        // await db.Database.EnsureDeletedAsync();
        await dbInitializer.InitializeDatabaseAsync(db, cancellationToken));

app.MapDefaultEndpoints();

app.Run();
