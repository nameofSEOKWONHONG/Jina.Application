using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Account;
using Jina.Domain.Entity.Account;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using eXtensionSharp;
using Jina.Domain.Abstract.Account.Token;

namespace Jina.Domain.Service.Account.Token
{
    public class GenerateJwtTokenService : ServiceImplBase<GenerateJwtTokenService, User, string>, IGenerateJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IGenerateEncryptedTokenService _generateEncryptedTokenService;
        private readonly IGetClaimsService _getClaimsService;
        private readonly IGetSigningCredentialsService _getSigningCredentialsService;

        public GenerateJwtTokenService(IConfiguration configuration,
            IGenerateEncryptedTokenService generateEncryptedTokenService,
            IGetClaimsService getClaimsService,
            IGetSigningCredentialsService getSigningCredentialsService) : base()
        {
            _configuration = configuration;
            _generateEncryptedTokenService = generateEncryptedTokenService;
            _getClaimsService = getClaimsService;
            _getSigningCredentialsService = getSigningCredentialsService;
        }

        public override Task<bool> OnExecutingAsync()
        {
            return Task.FromResult(true);
        }

        public override async Task OnExecuteAsync()
        {
            SigningCredentials signingCredentials = null;
            IEnumerable<Claim> claims = null;
            await ServiceInvoker<User, IEnumerable<Claim>>.Invoke(_getClaimsService)
                .AddFilter(() => Request.xIsNotEmpty())
                .SetParameter(() => Request)
                .OnExecutedAsync((res) => claims = res);

            await ServiceInvoker<bool, SigningCredentials>.Invoke(_getSigningCredentialsService)
                .AddFilter(() => claims.xIsNotEmpty())
                .SetParameter(() => true)
                .OnExecutedAsync((res) => signingCredentials = res);

            await ServiceInvoker<IdentityGenerateEncryptedTokenRequest, string>.Invoke(_generateEncryptedTokenService)
                .AddFilter(() => claims.xIsNotEmpty())
                .AddFilter(() => signingCredentials.xIsNotEmpty())
                .SetParameter(() => new IdentityGenerateEncryptedTokenRequest()
                {
                    SigningCredentials = signingCredentials,
                    Claims = claims
                })
                .OnExecutedAsync((res) =>
                {
                    Result = res;
                });
        }
    }
}