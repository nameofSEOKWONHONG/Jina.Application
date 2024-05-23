using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Example;

public interface IUpdateWeatherService : IServiceImplBase<UpdateWeatherRequest, IResults<int>>, IScopeService
{
    
}