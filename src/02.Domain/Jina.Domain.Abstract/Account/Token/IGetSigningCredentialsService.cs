using Jina.Base.Service.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Account.Token
{
    public interface IGetSigningCredentialsService : IServiceImplBase<bool, SigningCredentials>, IScopeService
    {
    }
}