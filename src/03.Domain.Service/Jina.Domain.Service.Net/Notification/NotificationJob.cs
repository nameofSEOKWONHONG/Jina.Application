using Microsoft.AspNetCore.SignalR;

namespace Jina.Domain.Service.Net.Notification;

public class NotificationJob
{
    private readonly IHubContext<MessageHub> _hubContext;

    public NotificationJob(IHubContext<MessageHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void Execute(string user, string message)
    {
        //만약, 엑셀 다운로드라면...

        //처리해서 엑셀 생성하고

        //스토리지에 업로드 후

        //임시 주소 획득해서

        //사용자에게 URL포함한 메세지를 전송한다.
        _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message)
            .GetAwaiter()
            .GetResult();
    }
}