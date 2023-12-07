using Jina.Domain.Infra.Base;
using Jina.Passion.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Passion.Api.Controllers.Notification
{
    public class NotificationController : JControllerBase
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public NotificationController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "test", "test");
            return Ok();
        }
    }
}