using System.Text.Json;
using BuildingBlocks.Diagnostics;
using Contracts;
using DotNetCore.CAP;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace Notifications;

public class TodoCreatedEventConsumer(
    ILogger<TodoCreatedEventConsumer> logger,
    IFluentEmail fluentEmail) : ICapSubscribe
{

    [CapSubscribe(nameof(TodoCreatedEvent))]
    public async Task ProcessAsync(TodoCreatedEvent todoCreatedEvent)
    {
        using var activity = Instrumentation.Source.StartActivity(nameof(TodoCompletedEventConsumer));

        logger.LogInformation("(TodoCreatedEventConsumer) --- Todo {TodoId} has been created, ", todoCreatedEvent.TodoId);

        await fluentEmail
            .To("hellO@dotnet.com")
            .Subject("Todo Created")
            .Body(JsonSerializer.Serialize(todoCreatedEvent))
            .SendAsync();

        logger.LogInformation("(TodoCreatedEventConsumer) --- Todo {TodoId} has been sent to email", todoCreatedEvent.TodoId);
    }
}
