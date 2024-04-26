using Microsoft.AspNetCore.Http;

namespace Jina.Domain.Service.Infra.Extension;

public static class ResponseExtensions
{
    public static async Task vStreamResponseAsync(this HttpResponse httpResponse, Func<HttpResponse, Task> response)
    {
        httpResponse.Headers.Append("Content-Type", "text/event-stream");
        httpResponse.Headers.Append("Cache-Control", "no-cache");
        httpResponse.Headers.Append("Connection", "keep-alive");

        await response(httpResponse);
    }
}