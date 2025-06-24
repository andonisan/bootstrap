
using Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Notifications.Infrastructure.Hubs;

public interface ITodoHub
{
    Task SendCompleted(TodoCompleted todoCompleted);

    Task SendWorker(string worker);
}

public class TodoHub : Hub<ITodoHub>, ITodoHub
{
    public async Task SendCompleted(TodoCompleted todoCompleted) => await Clients.All.SendCompleted(todoCompleted);

    public Task SendWorker(string worker) => Clients.All.SendWorker(worker);
}
