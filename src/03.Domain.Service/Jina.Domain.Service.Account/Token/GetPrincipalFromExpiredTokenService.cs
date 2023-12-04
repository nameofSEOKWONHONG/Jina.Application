using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Entity;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eXtensionSharp;

namespace Jina.Domain.Service.Account.Token
{
    public interface IGetPrincipalFromExpiredTokenService : IServiceImplBase<string, ClaimsPrincipal>, IScopeService
    {
    }

    public class GetPrincipalFromExpiredTokenService : EfServiceImpl<GetPrincipalFromExpiredTokenService, string, ClaimsPrincipal>, IGetPrincipalFromExpiredTokenService
    {
        private readonly IConfiguration _configuration;

        public GetPrincipalFromExpiredTokenService(AppDbContext dbContext, IConfiguration configuration) : base(dbContext, null)
        {
            _configuration = configuration;
        }

        public override Task<bool> OnExecutingAsync()
        {
            return Task.FromResult(true);
        }

        public override Task OnExecuteAsync()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration.GetValue<string>("Application:Secret")
                            .xToSHA512Decrypt(ApplicationConsts.Encryption.DB_ENC_SHA512_KEY))),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(this.Request, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            this.Result = principal;

            return Task.CompletedTask;
        }
    }
}