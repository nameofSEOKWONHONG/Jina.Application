using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.Shared;

namespace Jina.Domain.Abstract.Example;

public interface IGetPdfWeathersService : IServiceImplBase<PaginatedRequest<WeatherForecastRequest>, Results<byte[]>>
    , IScopeService
{
    
}