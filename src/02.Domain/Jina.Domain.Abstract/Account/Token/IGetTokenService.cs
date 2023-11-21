using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Abstract.Account.Token
{
    public interface IGetTokenService
        : IServiceImplBase<TokenRequest, IResultBase<TokenResponse>>
            , IScopeService
    {
    }
}