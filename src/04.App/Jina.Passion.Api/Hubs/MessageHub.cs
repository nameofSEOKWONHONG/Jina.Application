using eXtensionSharp;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Passion.Api.Hubs
{
    public interface IHubMessage<T>
    {
        ENUM_MESSAGE_TYPE MessageType { get; set; }
        T Data { get; set; }
    }

    public class HubMessage<T> : IHubMessage<T>
    {
        public ENUM_MESSAGE_TYPE MessageType { get; set; }
        public T Data { get; set; }
    }

    public class ProtocalHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //public async Task SendMessage<T>(string user, IHubMessage<T> message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message.xToJson());
        //}
    }

    public enum ENUM_MESSAGE_TYPE {
        Message,
        Notice,
        EmailSended
    }
}