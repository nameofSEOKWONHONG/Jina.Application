using Jina.Base.Service;
using Jina.Domain.Notification;
using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Domain.Service.Net.Notification;

public class NotificationJob : JobBase<MessageHub, SendMessageRequest>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spl"></param>
    /// <param name="hubContext"></param>
    public NotificationJob(ServicePipeline spl
        , IHubContext<MessageHub> hubContext) : base(spl, hubContext)
    {
        
    }


    public override async Task ExecuteAsync(SendMessageRequest request)
    {
        //만약, 엑셀 다운로드라면...

        //처리해서 엑셀 생성하고

        //스토리지에 업로드 후

        //임시 주소 획득해서

        //사용자에게 URL포함한 메세지를 전송한다.
        await this.HubContext.Clients
            .All
            .SendAsync(request.Method, request.UserId, request.Message);
    }
}