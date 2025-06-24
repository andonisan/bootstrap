using System.Data;
using BuildingBlocks.DependencyInjection;
using BuildingBlocks.DomainEvents;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Todos.Diagnostics;

namespace Todos;

public static class TodosModule
{
    public static WebApplicationBuilder Install(WebApplicationBuilder builder)
    {
        ConfigureTodosDatabase(builder);

        builder.Services.AddSingleton<Diagnostics.Diagnostics>();

        builder.Services.AddOpenTelemetry()
            .WithMetrics(x => x.AddMeter(Instrumentation.ServiceName))
            .WithTracing(x => x.AddSource(Instrumentation.Source.Name));

        builder.AddAssemblyServices(typeof(TodosModule).Assembly);

        return builder;
    }

    private static void ConfigureTodosDatabase(WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("TodoAppDb");

        builder.Services.AddDbContext<TodoDbContext>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging();

            var publishDomainEventsInterceptor =
                serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>();

            optionsBuilder.AddInterceptors(publishDomainEventsInterceptor);
        });

        builder.EnrichNpgsqlDbContext<TodoDbContext>();

        builder.Services.AddDbContext<ReadOnlyTodoDbContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddTransient<IDbConnection>(db => new NpgsqlConnection(connectionString));
    }
}
