using Jina.Domain.Shared.Abstract;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Jina.Passion.Mud.Client.Features.Http.Abstract;

public interface IRestClient
{
    HttpClient HttpClient { get; }

    Task<TResult> ExecuteAsync<TRequest, TResult>(HttpMethod method, string url, TRequest body, BrowserRequestCache cache = BrowserRequestCache.Default);

    Task PostFormDataAsync<TResponse>(string url, Action<MultipartFormDataContent> contentSetAction, Action<TResponse> handler)
        where TResponse : IResults;
}