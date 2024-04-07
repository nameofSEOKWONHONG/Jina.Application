using System.Collections.Generic;
using eXtensionSharp;
using Jina.Domain.SharedKernel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Passion.Client.Base
{
    public abstract class RequestManagerBase
    {
        protected readonly HttpClient Client;

        protected RequestManagerBase(HttpClient client)
        {
            Client = client;
        }
    }

    public abstract class PaginatedRequestManagerBase<TRequest, TResult> : RequestManagerBase
        where TResult : DisplayRow
    {
        protected PaginatedRequestManagerBase(HttpClient client) : base(client)
        {
        }

        public virtual async Task<PaginatedResult<TResult>> GetAll(string url, PaginatedRequest<TRequest> request)
        {
            var res = await Client.GetAsync($"api/{url}?{GenerateParam(request)}");
            var result = await res.Content.ReadFromJsonAsync<PaginatedResult<TResult>>();
            for (var i = 0; i < result.Data.Count; i++)
            {
                var row = result.Data[i];
                row.RowClass = (i + 1) % 2 == 0 ? "second" : "first";
            }

            return result;
        }

        private string GenerateParam(PaginatedRequest<TRequest> request)
        {
            if (request.xIsEmpty()) return string.Empty;
            var result = new List<string>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(request))
            {
                result.Add(property.Name + "=" + property.GetValue(request));
            }
            return string.Join("&", result);
        }

        public virtual async Task<PaginatedResult<TResult>> GetAllByPost(string url, PaginatedRequest<TRequest> request)
        {
            var res = await Client.PostAsync($"api/{url}", new StringContent(request.xToJson(), Encoding.UTF8, "application/json"));
            var result = await res.Content.ReadFromJsonAsync<PaginatedResult<TResult>>();
            for (var i = 0; i < result.Data.Count; i++)
            {
                var row = result.Data[i];
                row.RowClass = (i + 1) % 2 == 0 ? "second" : "first";
            }

            return result;
        }
    }
}