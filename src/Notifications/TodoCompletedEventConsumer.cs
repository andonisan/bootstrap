using System.Text.Json;
using BuildingBlocks.Diagnostics;
using Contracts;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using FluentEmail.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notifications.Infrastructure.Hubs;

namespace Notifications;

public class TodoCompletedEventConsumer(
    ILogger<TodoCompletedEventConsumer> logger,
    IFluentEmail fluentEmail,
    IHubContext<TodoHub, ITodoHub> hubContext) : ICapSubscribe
{

    [CapSubscribe(nameof(TodoCompletedEvent))]
    public async Task ProcessAsync(TodoCompletedEvent todoCompletedEvent)
    {
        using var activity = Instrumentation.Source.StartActivity(nameof(TodoCompletedEventConsumer));


        logger.LogInformation("(NOTIFICATIONS) --- Todo {TodoId} has been completed, ", todoCompletedEvent.Id);

        await hubContext.Clients.All.SendCompleted(new TodoCompleted
        {
            Title = todoCompletedEvent.Title
        });

        await fluentEmail
            .To("hellO@dotnet.com")
            .Subject("Todo Completed")
            .Body(JsonSerializer.Serialize(todoCompletedEvent))
            .SendAsync();

        logger.LogInformation("(NOTIFICATIONS) --- Notification sent to all clients for Todo {TodoId}",
            todoCompletedEvent.Id);
    }
}
