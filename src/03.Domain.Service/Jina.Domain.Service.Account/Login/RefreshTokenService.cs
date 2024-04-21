using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jina.Domain.Service.Account;

[TransactionOptions()]
public sealed class RefreshTokenService : ServiceImplBase<RefreshTokenService, AppDbContext, RefreshTokenRequest, IResults<TokenResult>>,
    IRefreshTokenService
{
    private readonly ApplicationConfig _config;
    private Entity.Account.User _user;
    private readonly IGenerateEncryptedTokenService _generateEncryptedTokenService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    /// <param name="context"></param>
    public RefreshTokenService(ISessionContext context, ServicePipeline svc,
        IGenerateEncryptedTokenService generateEncryptedTokenService,
        IOptions<ApplicationConfig> options) : base(context, svc)
    {
        _generateEncryptedTokenService = generateEncryptedTokenService;
        _config = options.Value;
    }

    public override async Task OnExecutingAsync()
    {
        if (this.Request.xIsEmpty())
        {
            this.Result =  await Results<TokenResult>.FailAsync("Invalid Client Token.");
        }
        
        var userPrincipal = GetPrincipalFromExpiredToken(this.Request.Token);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        _user = await Db.Users.FirstOrDefaultAsync(m => m.TenantId == this.Ctx.TenantId && m.Email == userEmail);
        
        if (_user.xIsEmpty())
            this.Result = await Results<TokenResult>.FailAsync("User Not Found.");

        if (_user.RefreshToken != this.Request.RefreshToken || _user.RefreshTokenExpiryTime <= DateTime.Now)
            this.Result = await Results<TokenResult>.FailAsync("Invalid Client Token.");        
    }

    public override async Task OnExecuteAsync()
    {
        string token = string.Empty;
        this.Svc.Register(_generateEncryptedTokenService)
            .SetParameter(() => _user)
            .OnExecuted(r => token = r);
        await this.Svc.ExecuteAsync();
        
        _user.RefreshToken = GenerateRefreshToken();
        this.Db.Users.Update(_user);
        await this.Db.SaveChangesAsync();

        var response = new TokenResult { Token = token, RefreshToken = _user.RefreshToken, RefreshTokenExpiryTime = _user.RefreshTokenExpiryTime };
        this.Result = await Results<TokenResult>.SuccessAsync(response);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}