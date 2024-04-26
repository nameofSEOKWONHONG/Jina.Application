using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface ILoginService : IServiceImplBase<TokenRequest, IResults<TokenResult>>, IScopeService
{

}