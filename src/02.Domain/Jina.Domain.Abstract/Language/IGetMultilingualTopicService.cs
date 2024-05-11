using Jina.Base.Service.Abstract;
using Jina.Domain.Multilingual;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Language;

public interface IGetMultilingualTopicService 
    : IServiceImplBase<GetMultilingualTopicRequest, IResults<GetMultilingualTopicResult>>, IScopeService
{
    
} 