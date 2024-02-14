using Jina.Base.Service.Abstract;
using Jina.Domain.Net.ExchangeRate;

namespace Jina.Domain.Abstract.Net.ExchangeRate;

public interface IExchangeRateService
    : IServiceImplBase<ExchangeRequest, IEnumerable<ExchangeResult>>
    , ISingletonService
{
    
}