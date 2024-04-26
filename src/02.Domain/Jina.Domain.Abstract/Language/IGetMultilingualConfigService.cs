using Jina.Base.Service.Abstract;
using Jina.Domain.Multilingual;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Language;

public interface IGetMultilingualConfigService : IServiceImplBase<bool, IResults<GetMultilingualConfigResult>>, IScopeService
{
    
} 