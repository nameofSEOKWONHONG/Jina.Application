using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface ILoginService : IServiceImplBase<TokenRequest, IResults<TokenResult>>, IScopeService
{

}