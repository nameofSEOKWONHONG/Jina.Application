using ChatGPT.Net;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.OpenApi.Services;

namespace Jina.Domain.Service.Net.OpenApi;

public class OpenApiClientService
    : ServiceImplCore<OpenApiClientService, string, string>
        , IOpenApiClientService
{
    public OpenApiClientService()
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