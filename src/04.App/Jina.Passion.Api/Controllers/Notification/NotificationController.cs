using eXtensionSharp;
using Hangfire;
using Jina.Domain.Notification;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Net.Notification;
using Jina.Validate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Results = Jina.Domain.Shared.Results;

namespace Jina.Passion.Api.Controllers.Notification;

public class NotificationController : JControllerBase
{
    private readonly IHubContext<MessageHub> _hubContext;
    private readonly IBackgroundJobClient _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="client"></param>
    public NotificationController(IHubContext<MessageHub> hubContext,
        IBackgroundJobClient client)
    {
        _hubContext = hubContext;
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageRequest request,
        [FromServices] SendMessageRequestValidator validator)
    {
        var valid = await validator.ValidateAsync(request);
        if (valid.IsValid.xIsFalse())
        {
            return Ok(await Results.FailAsync(valid.vToKeyValueErrors()));
        }
        await _hubContext.Clients.All.SendAsync(request.Method, request.UserId, request.Message);
        return Ok(await Results.SuccessAsync());
    }

    [HttpPost]
    public IActionResult CreateJob()
    {
        var request = new SendMessageRequest()
        {
            Method = "ReceiveMessage",
            UserId = "test",
            Message = "test"
        };
        _client.Enqueue<NotificationJob>(m => m.ExecuteAsync(request));
        return Ok();
    }
}