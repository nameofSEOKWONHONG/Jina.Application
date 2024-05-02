using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Example
{
    public interface IGetWeatherService : IServiceImplBase<int, IResults<WeatherForecastResult>>, IScopeService
    {
    }
}