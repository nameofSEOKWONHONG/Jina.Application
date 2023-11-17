using eXtensionSharp;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Base;
using Jina.Domain.Base.Abstract;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.Const;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Account;

public class GetTokenService
    : InfraServiceImpl<GetTokenService, TokenRequest, IResultBase<TokenResponse>>
        , IGetTokenService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public GetTokenService(AppDbContext db, ISessionContext context
        , IPasswordHasher<User> passwordHasher) : base(db, context)
    {
        _passwordHasher = passwordHasher;
    }

    public override async Task<bool> OnExecutingAsync()
    {
        var users = this.DbContext.Set<User>();
        var user = await users.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == this.SessionContext.TenantId &&
                                                                       m.Email == this.Request.Email);
        if (user.xIsEmpty())
        {
            this.Result = await Result<TokenResponse>.FailAsync("Access failed.");
            return false;
        }

        if (user.AccessFailedCount > ApplicationConsts.Limit.ACCESS_LIMIT_COUNT)
        {
            this.Result = await Result<TokenResponse>.FailAsync("Access failed 5 times. Contact administrator.");
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, this.Request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            user.AccessFailedCount += 1;
            users.Update(user);
            await this.DbContext.SaveChangesAsync();
            this.Result = await Result<TokenResponse>.FailAsync("Access failed.");
            return false;
        }

        return true;
    }

    public override Task OnExecuteAsync()
    {
        return Task.CompletedTask;
    }
}