using Hangfire;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Jina.Domain.Infra.Base;
using Jina.Passion.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;

namespace Jina.Passion.Api.Controllers.Notification
{
    public class NotificationController : JControllerBase
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IBackgroundJobClient _client;

        public NotificationController(IHubContext<MessageHub> hubContext,
            IBackgroundJobClient client)
        {
            _hubContext = hubContext;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "test", "test");
            return Ok();
        }

        [HttpGet]
        public IActionResult CreateJob()
        {
            _client.Enqueue<MyBackgroundJob>(m => m.Execute("test1", "test1"));
            return Ok();
        }
    }

    public class MyBackgroundJob
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MyBackgroundJob(IHubContext<MessageHub> hubContext)
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
}