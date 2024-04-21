using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Database.Abstract;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jina.Domain.Service.Account;

[TransactionOptions()]
public sealed class GenerateEncryptedTokenService : ServiceImplBase<GenerateEncryptedTokenService, Entity.Account.User, string>, 
    IGenerateEncryptedTokenService
{
    private readonly IGetClaimsService _getClaimsService;
    private readonly ApplicationConfig _config;
    
    /// <summary>
    /// JWT 토큰 생성 서비스
    /// </summary>
    /// <param name="db"></param>
    /// <param name="context"></param>
    public GenerateEncryptedTokenService(ISessionContext ctx, ServicePipeline svc,
        IOptions<ApplicationConfig> options, IGetClaimsService getClaimsService) : base(ctx, svc)
    {
        _config = options.Value;
        _getClaimsService = getClaimsService;
    }

    public override Task OnExecutingAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task OnExecuteAsync()
    {
        IEnumerable<Claim> claims = null;
        this.Svc.Register(_getClaimsService)
            .SetParameter(() => this.Request)
            .OnExecuted(r => claims = r);

        await this.Svc.ExecuteAsync();
        
        var secret = Encoding.UTF8.GetBytes(_config.Secret);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        this.Result = encryptedToken;
    }
}