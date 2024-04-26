using System.Net;
using eXtensionSharp;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Results = Jina.Domain.Shared.Results;

namespace Jina.Domain.Service.Infra.Middleware;

public class GlobalErrorHandler : IExceptionHandler
{
    private readonly ILogger _logger;

    public GlobalErrorHandler(ILogger<GlobalErrorHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context
        , Exception exception
        , CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred : {Message}", exception.Message);

        var problem = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Type = string.Empty
        };
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(await Results.FailAsync(exception.Message), cancellationToken: cancellationToken);

        return true;
    }
}