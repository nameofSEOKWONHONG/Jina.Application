using Jina.Base.Service.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface IGenerateEncryptedTokenService : IServiceImplBase<Entity.Account.User, string>, IScopeService
{
    
}