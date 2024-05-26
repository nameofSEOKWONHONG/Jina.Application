using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Example;

public interface ISaveWeatherService : IServiceImplBase<WeatherForecastResult, IResults<int>>, IScopeService
{
        
}

public interface ISaveFreeWeatherService : IServiceImplBase<WeatherForecastResult, IResults<int>>, IScopeService
{
        
}