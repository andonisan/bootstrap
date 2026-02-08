using Contracts;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace Todos.Application.Features.Todo.Events;

internal class TodoCreatedEventHandler(ICapPublisher publishEndpoint, ILogger<TodoCreatedEventHandler> logger)
    : INotificationHandler<TodoCreatedEvent>
{
    public async Task Handle(TodoCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("(TODOS - DOMAIN) Created todo {TodoId}", notification.TodoId);

        await publishEndpoint.PublishAsync(nameof(TodoCreatedEvent), notification,
            cancellationToken: cancellationToken);
    }
}
