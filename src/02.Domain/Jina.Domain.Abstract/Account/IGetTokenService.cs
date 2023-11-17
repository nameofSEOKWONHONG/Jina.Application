using Jina.Base.Service.Abstract;
using Jina.Domain.Account;
using Jina.Domain.Kernel.Abstract;

namespace Jina.Domain.Abstract.Account
{
    public interface IGetTokenService
        : IServiceImplBase<TokenRequest, IResultBase<TokenResponse>>
            , IScopeService
    {
    }
}