using Jina.Domain.Account.Token;
using Jina.Domain.Shared.Abstract;

namespace Jina.Passion.Client.Services.Account;

public interface IAccountService
{
    Task<IResults<TokenResult>> Login(TokenRequest request);
    Task<string> TryRefreshToken();
    Task<IResults> Logout();
}