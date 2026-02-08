using Contracts;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace Todos.Application.Features.Todo.Events;

internal class TodoCompletedEventHandler(ICapPublisher publishEndpoint, ILogger<TodoCompletedEventHandler> logger)
    : INotificationHandler<TodoCompletedEvent>
{
    public async Task Handle(TodoCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("(TODOS - DOMAIN EVENT) --- Todo {TodoId} has been completed", notification.Id);

        await publishEndpoint.PublishAsync(nameof(TodoCompletedEvent), notification,
            cancellationToken: cancellationToken);
    }
}
