using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Account.User;

public sealed class CreateRoleClaimService : ServiceImplBase<CreateRoleClaimService, AppDbContext, CreateRoleClaimRequest, IResults<bool>>
    , IScopeService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    /// <param name="pipe"></param>
    public CreateRoleClaimService(ILogger<CreateRoleClaimService> logger, ISessionContext context, ServicePipeline pipe) : base(logger, context, pipe)
    {
    }

    public override async Task<bool> OnExecutingAsync()
    {
        var exist = await this.Db.RoleClaims
            .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                                      m.Id == this.Request.Id &&
                                      m.RoleId == this.Request.RoleId &&
                                      m.ClaimType == this.Request.Type &&
                                      m.ClaimValue == this.Request.Value);

        if (exist.xIsNotEmpty())
        {
            this.Result = await Results<bool>.FailAsync("already exists");
            return false;
        }

        return true;
    }

    public override async Task OnExecuteAsync()
    {
        var roleClaim = new RoleClaim()
        {
            TenantId = this.Request.TenantId,
            RoleId = this.Request.RoleId,
            ClaimType = this.Request.Type,
            ClaimValue = this.Request.Value,
            Description = this.Request.Description,
            Group = this.Request.Group
        };
        await this.Db.RoleClaims.AddAsync(roleClaim);
        await this.Db.SaveChangesAsync();
        this.Result = await Results<bool>.SuccessAsync("Role Claim created.");
    }
}