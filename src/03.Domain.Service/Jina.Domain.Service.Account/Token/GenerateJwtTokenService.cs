using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace Jina.Domain.Service.Account.Token
{
    public class GenerateJwtTokenService : ServiceImplCore<GenerateJwtTokenService, Entity.Account.User, string>, IGenerateJwtTokenService
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
            await ServicePipeline<Entity.Account.User, IEnumerable<Claim>>.Create(_getClaimsService)
                .AddFilter(() => Request.xIsNotEmpty())
                .SetParameter(() => Request)
                .OnExecutedAsync((res) => claims = res);

            await ServicePipeline<bool, SigningCredentials>.Create(_getSigningCredentialsService)
                .AddFilter(() => claims.xIsNotEmpty())
                .SetParameter(() => true)
                .OnExecutedAsync((res) => signingCredentials = res);

            await ServicePipeline<IdentityGenerateEncryptedTokenRequest, string>.Create(_generateEncryptedTokenService)
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