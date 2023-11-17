using Jina.Domain.Account;
using Jina.Domain.Base.Abstract;

namespace Jina.Passion.FE.Client.Base.Abstract.Interfaces
{
    public interface ITokenManager
    {
        Task<IResultBase> Login(TokenRequest model);
    }
}