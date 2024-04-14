using System.Data.Entity;
using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Account.User;

public class CreateRoleClaimService : ServiceImplBase<CreateRoleClaimService, AppDbContext, CreateRoleClaimRequest, IResultBase<bool>>
    , IScopeService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="svc"></param>
    public CreateRoleClaimService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
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
            this.Result = await ResultBase<bool>.FailAsync("already exists");
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
        this.Result = await ResultBase<bool>.SuccessAsync("Role Claim created.");
    }
}