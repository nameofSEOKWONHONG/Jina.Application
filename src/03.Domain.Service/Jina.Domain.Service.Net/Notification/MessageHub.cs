using Microsoft.AspNetCore.SignalR;

namespace Jina.Domain.Service.Net.Notification;

/// <summary>
/// 
/// </summary>
public class MessageHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}