using Jina.Base.Service.Abstract;

namespace Jina.Domain.Abstract.Net.OpenApi.Services;

public interface IOpenApiClientService
    : IServiceImplBase<string, string>
        , IScopeService
{
    
}
