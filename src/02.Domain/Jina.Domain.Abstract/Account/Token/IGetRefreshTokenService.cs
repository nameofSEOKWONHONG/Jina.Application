using Jina.Base.Service.Abstract;
using Jina.Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Account.Token
{
    public interface IGetRefreshTokenService : IServiceImplBase<User, string>, IScopeService
    {
    }
}