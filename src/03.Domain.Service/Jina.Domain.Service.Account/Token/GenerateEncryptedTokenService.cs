using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Account.Token;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Service.Account.Token
{
    public class GenerateEncryptedTokenService : ServiceImplCore<GenerateEncryptedTokenService, IdentityGenerateEncryptedTokenRequest, string>, IGenerateEncryptedTokenService
    {
        public GenerateEncryptedTokenService() : base()
        {
        }

        public override Task<bool> OnExecutingAsync()
        {
            return Task.FromResult(true);
        }

        public override Task OnExecuteAsync()
        {
            var token = new JwtSecurityToken(
                claims: Request.Claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: Request.SigningCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            Result = encryptedToken;
            return Task.CompletedTask;
        }
    }
}