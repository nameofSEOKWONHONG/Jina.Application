using System.Security.Claims;
using Jina.Base.Service.Abstract;

namespace Jina.Domain.Abstract.Account;

public interface IGetClaimsService : IServiceImplBase<Entity.Account.User, IEnumerable<Claim>>, IScopeService
{
    
}