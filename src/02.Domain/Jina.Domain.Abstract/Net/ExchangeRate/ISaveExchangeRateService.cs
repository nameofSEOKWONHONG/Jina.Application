using Jina.Base.Service.Abstract;
using Jina.Domain.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate.Enums;

namespace Jina.Domain.Abstract.Net.ExchangeRate;

public interface ISaveExchangeRateService
    : IServiceImplBase<bool, bool>
    , IScopeService
{
    
}