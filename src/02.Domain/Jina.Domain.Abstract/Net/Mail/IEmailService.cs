using Jina.Base.Service.Abstract;
using Jina.Domain.Net;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Abstract.Net;

public interface IEmailService : IServiceImplBase<EmailRequest, IResults<bool>>, IScopeService
{
    
}