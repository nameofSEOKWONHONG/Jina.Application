using Jina.Domain.Account.Token;
using Jina.Domain.Shared.Abstract;

namespace Jina.Passion.Client.Base.Abstract
{
    public interface ITokenManager
    {
        Task<IResults> Login(TokenRequest model);
    }
}