using Jina.Base.Service.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Account.Token
{
    public class IdentityGenerateEncryptedTokenRequest
    {
        public SigningCredentials SigningCredentials { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }

    public interface IGenerateEncryptedTokenService : IServiceImplBase<IdentityGenerateEncryptedTokenRequest, string>, IScopeService
    {
    }
}