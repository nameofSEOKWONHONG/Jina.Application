using eXtensionSharp;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Account;

public sealed class LogoutService : ServiceImplBase<LogoutService, AppDbContext, LogoutRequest, IResults<bool>>, ILogoutService
{
    private Entity.Account.User _user;
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="ctx"></param>
    public LogoutService(ISessionContext ctx) : base(ctx)
    {
        
    }

    public override async Task OnExecutingAsync()
    {
        _user = await this.Db.Users.FirstOrDefaultAsync(m => m.TenantId == this.Ctx.TenantId &&
                                                             m.Email == this.Ctx.CurrentUser.Email);
        
        if (_user.xIsEmpty()) this.Result = await Results<bool>.FailAsync("User not exist");
    }

    public override async Task OnExecuteAsync()
    {
        await this.Db.Users.Where(m => m.TenantId == _user.TenantId)
            .ExecuteUpdateAsync(m =>
                m.SetProperty(mm => mm.RefreshToken, string.Empty)
                    .SetProperty(mm => mm.RefreshTokenExpiryTime, DateTime.MinValue)
                    .SetProperty(mm => mm.LastModifiedBy, this.Ctx.CurrentUser.UserId)
                    .SetProperty(mm => mm.LastModifiedOn, this.Ctx.CurrentTime.Now)
            );

        await this.Db.SaveChangesAsync();
        this.Result = await Results<bool>.SuccessAsync();
    }
}