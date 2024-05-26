using eXtensionSharp;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Account;

public sealed class LogoutService : ServiceImplBase<LogoutService, AppDbContext, LogoutRequest, IResults<bool>>, ILogoutService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="ctx"></param>
    public LogoutService(ILogger<LogoutService> logger, ISessionContext ctx) : base(logger, ctx)
    {
    }

    public override async Task<bool> OnExecutingAsync()
    {
        var user = await this.Db.Users.FirstOrDefaultAsync(m => m.TenantId == this.Context.TenantId &&
                                                             m.Email == this.Context.CurrentUser.Email);

        if (user.xIsEmpty())
        {
            this.Result = await Results<bool>.FailAsync("User not exist");
            return false;
        }

        return true;
    }

    public override async Task OnExecuteAsync()
    {
        var user = await this.Db.Users.FirstOrDefaultAsync(m => m.TenantId == this.Context.TenantId &&
                                                                m.Email == this.Context.CurrentUser.Email);
        
        await this.Db.Users.Where(m => m.TenantId == user.TenantId)
            .ExecuteUpdateAsync(m =>
                m.SetProperty(mm => mm.RefreshToken, string.Empty)
                    .SetProperty(mm => mm.RefreshTokenExpiryTime, DateTime.MinValue)
                    .SetProperty(mm => mm.LastModifiedBy, this.Context.CurrentUser.UserId)
                    .SetProperty(mm => mm.LastModifiedOn, this.Context.CurrentTime.Now)
            );

        await this.Db.SaveChangesAsync();
        this.Result = await Results<bool>.SuccessAsync();
    }
}