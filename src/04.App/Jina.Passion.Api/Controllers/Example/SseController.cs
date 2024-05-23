using System.Text;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Infra.Extension;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example;

public class SseController : ActionController
{
    [HttpGet]
    public async Task Get(CancellationToken cancellationToken)
    {
        await this.Response.vStreamResponseAsync(async res =>
        {
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                // Your data to send
                var data = $"id: {this.HttpContext.Connection.Id} data: {random.Next()}\n\n";

                // Write data to response stream
                var buffer = Encoding.UTF8.GetBytes(data);
                await Response.Body.WriteAsync(buffer, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);

                // Delay between events
                await Task.Delay(1000, cancellationToken);
            }
        });
    }
}