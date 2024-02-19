using Jina.Base.Service.Abstract;
using Jina.Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Account.Token
{
    public interface IGetClaimsService : IServiceImplBase<Jina.Domain.Entity.Account.User, IEnumerable<Claim>>, IScopeService
    {
    }
}