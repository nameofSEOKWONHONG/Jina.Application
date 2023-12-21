using AntDesign;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Passion.Api.Hubs
{
    public class ProtocalHub : Hub
    {
        public async Task SendMessage(string user, string jsonString)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, jsonString);
        }
    }

    public class ProtocalBuilder {
        private ENUM_PROTOCAL_TYPE _protocalType;
        public ProtocalBuilder()
        {
            
        }

        public IProtocalBuilder Create(ENUM_PROTOCAL_TYPE type) {
            IProtocalBuilder protocalBuilder = type switch {
                ENUM_PROTOCAL_TYPE.Message => new MessageBuilder(),
                _ => throw new NotImplementedException()
            };
            return protocalBuilder;
        }        
    }

    public interface IProtocalBuilder {

    }

    public class MessageBuilder : IProtocalBuilder {
        public MessageBuilder()
        {
            
        }

        
    }

    public enum ENUM_PROTOCAL_TYPE {
        Message,
        Notice,
        EmailSended
    }
}