using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface IRefreshTokenService : IServiceImplBase<RefreshTokenRequest, IResults<TokenResult>>, IScopeService
{
    
} 