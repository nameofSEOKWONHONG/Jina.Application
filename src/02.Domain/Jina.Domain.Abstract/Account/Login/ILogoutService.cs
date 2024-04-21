using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface ILogoutService : IServiceImplBase<LogoutRequest, IResults<bool>>, IScopeService
{
    
}