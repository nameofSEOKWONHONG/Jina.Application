using System.Net;
using eXtensionSharp;
using Jina.Domain.SharedKernel;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI.Common;
using Results = Jina.Domain.SharedKernel.Results;

namespace Jina.Domain.Service.Infra.Middleware;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = await Results.FailAsync(error.Message);

            switch (error)
            {
                // case ApiException e:
                //     // custom application error
                //     response.StatusCode = (int)HttpStatusCode.BadRequest;
                //     break;

                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.OK;
                    break;
            }

            var result = responseModel.xToJson();
            await response.WriteAsync(result);
        }
    }
}