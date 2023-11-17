using Jina.Base.Service.Abstract;
using Jina.Domain.Base.Abstract;
using Jina.Domain.Service.Account;

namespace Jina.Domain.Abstract.Account
{
    public interface IGetTokenService
        : IServiceImplBase<TokenRequest, IResultBase<TokenResponse>>
            , IScopeService
    {
    }
}