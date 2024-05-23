using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Domain.Service.Net.Notification;

// https://medium.com/@kova98/real-time-apps-in-net-with-signalr-f4e0381771ab
// https://stackoverflow.com/questions/77477151/signalr-direct-message-to-single-user-in-angular
// https://www.c-sharpcorner.com/article/real-time-communication-with-websocket-protocol-in-asp-net-core/
// https://medium.com/swlh/scaling-signalr-core-web-applications-with-kubernetes-fca32d787c7d
// https://github.com/nameofSEOKWONHONG/encrypted-chat
/// <summary>
/// 
/// </summary>
public class MessageHub : Hub
{
    private readonly ISessionContext _ctx;
    public MessageHub(ISessionContext ctx)
    {
        _ctx = ctx;
    }
    
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendUserMessage(string userId, string message)
    {
        await Clients.User(userId).SendAsync("SendUserMessage", message);
    }

    /// <summary>
    /// 전체에 전송하고 UserID를 포함한다.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="message"></param>
    public async Task SendAllMessage(string user, string message)
    {
        await Clients.All.SendAsync("SendAllMessage", user, message);
    }
    
    public async Task JoinGroup(string group, string user)
    {

        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await Clients.Caller.SendAsync("JoinGroup", $"join {group}");
        await Clients.OthersInGroup(group).SendAsync("JoinGroup", $"{user} has joined the group {group}.");
    }
    
    public async Task GroupSendMessage(string group, string user, string message)
    {
        await Clients.Group(group).SendAsync("GroupSendMessage", user, message);
    }
}