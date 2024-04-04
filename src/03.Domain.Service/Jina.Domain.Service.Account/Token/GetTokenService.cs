using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.Service.Infra.Const;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Account.Token;

public class GetTokenService
    : ServiceImplBase<GetTokenService, TokenRequest, IResultBase<TokenResponse>>
        , IGetTokenService
{
    private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;
    private readonly IGetRefreshTokenService _getRefreshTokenService;
    private readonly IGenerateJwtTokenService _generateJwtTokenService;

    public GetTokenService(AppDbContext db
        , ISessionContext context
        , IPasswordHasher<Entity.Account.User> passwordHasher
        , IGetRefreshTokenService getRefreshTokenService
        , IGenerateJwtTokenService generateJwtTokenService) : base(db, context)
    {
        _passwordHasher = passwordHasher;
        _getRefreshTokenService = getRefreshTokenService;
        _generateJwtTokenService = generateJwtTokenService;
    }

    public override async Task<bool> OnExecutingAsync()
    {
        var users = DbContext.Set<Entity.Account.User>();
        var user = await users.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == SessionContext.TenantId &&
                                                                       m.Email == Request.Email);
        if (user.xIsEmpty())
        {
            Result = await Result<TokenResponse>.FailAsync("User not found.");
            return false;
        }

        if (user.AccessFailedCount > ApplicationConsts.Limit.ACCESS_LIMIT_COUNT)
        {
            Result = await Result<TokenResponse>.FailAsync("Access failed 5times. Contact administrator.");
            return false;
        }

        if (user.IsActive.xIsFalse())
        {
            Result = await Result<TokenResponse>.FailAsync("User not active. Please contact the administrator.");
            return false;
        }

        if (user.EmailConfirmed.xIsFalse())
        {
            Result = await Result<TokenResponse>.FailAsync("Email not confirmed.");
            return false;
        }

        if (user.TenantId != Request.TenantId)
        {
            Result = await Result<TokenResponse>.FailAsync("Tenant id not matched.");
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, Request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            user.AccessFailedCount += 1;
            users.Update(user);
            await DbContext.SaveChangesAsync();
            Result = await Result<TokenResponse>.FailAsync("Invalid credentials.");
            return false;
        }

        return true;
    }

    public override async Task OnExecuteAsync()
    {
        var users = DbContext.Set<Entity.Account.User>();
        var user = await users.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == SessionContext.TenantId && m.Email == Request.Email);

        string token = string.Empty;
        await ServicePipeline<Entity.Account.User, string>
            .Create(_getRefreshTokenService)
            .AddFilter(() => user.xIsNotEmpty())
            .SetParameter(() => user)
            .OnExecutedAsync(r =>
            {
                user.RefreshToken = r;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            });

        await ServicePipeline<Entity.Account.User, string>.Create(_generateJwtTokenService)
            .AddFilter(() => user.xIsNotEmpty())
            .SetParameter(() => user)
            .OnExecutedAsync((res) => token = res);

        var response = new TokenResponse()
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            UserImageURL = string.Empty
        };

        Result = await Result<TokenResponse>.SuccessAsync(response);

        user.AccessFailedCount = 0;
        users.Update(user);
        await DbContext.SaveChangesAsync();
    }
}