using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Jina.Domain.Service.Account.Token
{
	[TransactionOptions(System.Transactions.TransactionScopeOption.Required)]
    public class GetTokenRefreshService : ServiceImplBase<GetTokenRefreshService, RefreshTokenRequest, IResultBase<TokenResponse>>, IGetTokenRefreshService
    {
        private readonly IGetPrincipalFromExpiredTokenService _getPrincipalFromExpiredTokenService;
        private readonly IGetSigningCredentialsService _getSigningCredentialsService;
        private readonly IGetClaimsService _getClaimsService;
        private readonly IGenerateEncryptedTokenService _generateEncryptedTokenService;
        private readonly IGetRefreshTokenService _getRefreshTokenService;

        public GetTokenRefreshService(AppDbContext dbContext,
            IGetPrincipalFromExpiredTokenService getPrincipalFromExpiredTokenService,
            IGetSigningCredentialsService getSigningCredentialsService,
            IGetClaimsService getClaimsService,
            IGenerateEncryptedTokenService generateEncryptedTokenService,
            IGetRefreshTokenService getRefreshTokenService) : base(dbContext, null)
        {
            _getPrincipalFromExpiredTokenService = getPrincipalFromExpiredTokenService;
            _getSigningCredentialsService = getSigningCredentialsService;
            _getClaimsService = getClaimsService;
            _generateEncryptedTokenService = generateEncryptedTokenService;
            _getRefreshTokenService = getRefreshTokenService;
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Result<TokenResponse>.FailAsync("Invalid Client Token.");
                return false;
            }

            ClaimsPrincipal userPrincipal = null;
            await ServicePipeline<string, ClaimsPrincipal>.Create(_getPrincipalFromExpiredTokenService)
                .AddFilter(() => this.Request.xIsNotEmpty())
                .AddFilter(() => this.Request.Token.xIsNotEmpty())
                .SetParameter(() => this.Request.Token)
                .OnExecutedAsync((res) => userPrincipal = res);

            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await this.DbContext.Users.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId && m.Email == userEmail);

            if (user.xIsEmpty())
            {
                this.Result = await Result<TokenResponse>.FailAsync("User Not Found.");
                return false;
            }

            if (user.RefreshToken != this.Request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                this.Result = await Result<TokenResponse>.FailAsync("Invalid Client Token.");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            ClaimsPrincipal userPrincipal = null;
            await ServicePipeline<string, ClaimsPrincipal>.Create(_getPrincipalFromExpiredTokenService)
                .AddFilter(() => this.Request.xIsNotEmpty())
                .AddFilter(() => this.Request.Token.xIsNotEmpty())
                .SetParameter(() => this.Request.Token)
                .OnExecutedAsync((res) => userPrincipal = res);

            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await this.DbContext.Users.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId && m.Email == userEmail);

            string token = string.Empty;
            SigningCredentials signingCredentials = null;
            IEnumerable<Claim> claims = null;

            await ServicePipeline<bool, SigningCredentials>.Create(_getSigningCredentialsService)
                .AddFilter(() => true)
                .SetParameter(() => true)
                .OnExecutedAsync((res) => signingCredentials = res);

            await ServicePipeline<Entity.Account.User, IEnumerable<Claim>>.Create(_getClaimsService)
                .AddFilter(() => user.xIsNotEmpty())
                .SetParameter(() => user)
                .OnExecutedAsync((res) => claims = res);

            await ServicePipeline<IdentityGenerateEncryptedTokenRequest, string>.Create(_generateEncryptedTokenService)
                .AddFilter(() => claims.xIsNotEmpty())
                .SetParameter(() => new IdentityGenerateEncryptedTokenRequest()
                {
                    SigningCredentials = signingCredentials,
                    Claims = claims
                })
                .OnExecutedAsync((res) => token = res);

            await ServicePipeline<Entity.Account.User, string>.Create(_getRefreshTokenService)
                .AddFilter(() => user.xIsNotEmpty())
                .SetParameter(() => user)
                .OnExecutedAsync((res) => user.RefreshToken = res);

            if (token.xIsNotEmpty())
            {
                this.DbContext.Users.Update(user);
                await this.DbContext.SaveChangesAsync();

                var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
                this.Result = await Result<TokenResponse>.SuccessAsync(response);
            }
        }
    }
}