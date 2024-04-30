using eXtensionSharp;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text;
using Jina.Domain.Shared.Abstract;

namespace Jina.Passion.Client.Common.Infra
{
    public sealed class RestClient : IRestClient
    {
        public HttpClient HttpClient { get; private set; }

        public RestClient(HttpClient client, ILogger<RestClient> logger)
        {
            HttpClient = client;
        }

        public async Task<TResult> ExecuteAsync<TRequest, TResult>(HttpMethod method, string url, TRequest body, BrowserRequestCache cache = BrowserRequestCache.Default)
        {
            var createdUrl = CreateCachedUrl(url);
            var msg = new HttpRequestMessage(method, createdUrl);
            if(body.xIsNotEmpty())
            {
                msg.Content = new StringContent(body.xToJson(), Encoding.UTF8, "application/json");
            };

            msg.SetBrowserRequestCache(cache);
            var resp = await HttpClient.SendAsync(msg);
            if (resp.StatusCode != System.Net.HttpStatusCode.Unauthorized &&
                resp.StatusCode != System.Net.HttpStatusCode.Forbidden &&
                resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                resp.EnsureSuccessStatusCode(); //if error, throw exception;
            }

            // var stream = await resp.Content.ReadAsStreamAsync();
            // var temp = await stream.xToDecBase64Async();
            // var result = temp.xToEntity<TResponse>();

            var result = await resp.Content.ReadFromJsonAsync<TResult>();
            return result;
        }

        public async Task PostFormDataAsync<TResponse>(string url, Action<MultipartFormDataContent> contentSetAction, Action<TResponse> handler)
            where TResponse : IResultBase
        {
            if (contentSetAction.xIsEmpty())
                ArgumentException.ThrowIfNullOrEmpty("contentSetAction");

            if (handler.xIsEmpty()) throw new Exception("handler is null");

            using var multiPartFormData = new MultipartFormDataContent();
            contentSetAction(multiPartFormData);

            var resp = await HttpClient.PostAsync(url, multiPartFormData);
            if (resp.StatusCode != System.Net.HttpStatusCode.Unauthorized &&
                resp.StatusCode != System.Net.HttpStatusCode.Forbidden &&
                resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                resp.EnsureSuccessStatusCode(); //if error, throw exception;
            }

            // var stream = await resp.Content.ReadAsStreamAsync();
            // var temp = await stream.xToDecBase64Async();
            // var result = temp.xToEntity<TResponse>();

            var result = await resp.Content.ReadFromJsonAsync<TResponse>();

            handler(result);
        }

        /// <summary>
        /// url 캐시 관리
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string CreateCachedUrl(string url)
        {
            if (url.xContains("?"))
            {
                return $"{url}&v={DateTime.Now.ToString("yyyyMMddHHmmss")}";
            }
            return $"{url}?v={DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }
    }
}
