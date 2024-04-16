using Hangfire;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Net.Notification;
using Jina.Passion.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
            _client.Enqueue<NotificationJob>(m => m.Execute("test1", "test1"));
            return Ok();
        }
    }
}