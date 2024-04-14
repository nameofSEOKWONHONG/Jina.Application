using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Passion.Client.Services.Account;

public interface IAccountService
{
    Task<IResultBase<TokenResult>> Login(TokenRequest request);
    Task<string> TryRefreshToken();
    Task<IResultBase> Logout();
}