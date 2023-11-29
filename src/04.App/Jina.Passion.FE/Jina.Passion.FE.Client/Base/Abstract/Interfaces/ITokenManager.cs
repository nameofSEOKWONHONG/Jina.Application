using Jina.Domain.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Passion.FE.Client.Base.Abstract.Interfaces
{
    public interface ITokenManager
    {
        Task<IResultBase> Login(TokenRequest model);
    }
}