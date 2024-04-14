using System.Text.Json;
using System.Text.Json.Serialization;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Service.Infra.Extension;

public static class ResultExtensions
{
    public static async Task<IResultBase<T>> vToResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResultBase<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    public static async Task<IResultBase> vToResult(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResultBase>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    public static async Task<PaginatedResult<T>> vToPaginatedResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return responseObject;
    }
}