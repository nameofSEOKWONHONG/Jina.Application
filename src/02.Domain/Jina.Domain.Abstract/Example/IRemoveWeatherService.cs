using Jina.Base.Service.Abstract;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Example;

public interface IRemoveWeatherService : IServiceImplBase<int, IResults>, IScopeService
{
        
}

public interface IRemoveFreeWeatherService : IServiceImplBase<int, IResults>, IScopeService
{
        
}