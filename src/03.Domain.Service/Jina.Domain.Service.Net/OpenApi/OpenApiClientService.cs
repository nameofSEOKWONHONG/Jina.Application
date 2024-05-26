using ChatGPT.Net;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.OpenApi.Services;
using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Net.OpenApi;

public class OpenApiClientService
    : ServiceImplBase<OpenApiClientService, string, string>
        , IOpenApiClientService
{
    public OpenApiClientService(ILogger<OpenApiClientService> logger, ISessionContext context) : base(logger, context)
    {
    }

    public override Task<bool> OnExecutingAsync()
    {
        return Task.FromResult(true);
    }

    public override async Task OnExecuteAsync()
    {
        // ChatGPT Official API
        var bot = new ChatGpt("<API_KEY>");

        this.Result = await bot.Ask(this.Request);
    }
}