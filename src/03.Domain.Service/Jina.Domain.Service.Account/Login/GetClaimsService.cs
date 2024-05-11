using System.Security.Claims;
using System.Transactions;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Account;

public interface IGetClaimsService : IServiceImplBase<Entity.Account.User, IEnumerable<Claim>>, IScopeService
{
    
}

[TransactionOptions()]
public sealed class GetClaimsService : ServiceImplBase<GetClaimsService, AppDbContext, Entity.Account.User, IEnumerable<Claim>>,
    IGetClaimsService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public GetClaimsService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
    {
    }

    public override Task OnExecutingAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task OnExecuteAsync()
    {
        var userClaims = new List<Claim>();
        var tenant = await this.Db.Tenants.FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId);
        var userRole = await this.Db.UserRoles.FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId && m.UserId == this.Request.Id);
        var roles = await this.Db.Roles.Where(m => m.TenantId == this.Request.TenantId && m.Id == userRole.RoleId).ToListAsync();
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        await roles.xForEachAsync(async role =>
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
            var results = await this.Db.RoleClaims.Where(m => m.TenantId == this.Request.TenantId && m.RoleId == role.Id)
                .ToListAsync();
            var allPermissionsForThisRoles = new List<Claim>();
            results.ForEach(item =>
            {
                allPermissionsForThisRoles.Add(new Claim(item.ClaimType, item.ClaimValue));
            });
            permissionClaims.AddRange(allPermissionsForThisRoles);
        });

        var claims = new List<Claim>
            {
                new(ApplicationClaimTypes.TenantId, this.Request.TenantId),
                new(ApplicationClaimTypes.TimeZone, tenant.TimeZone),
                new(ClaimTypes.NameIdentifier, this.Request.Id),
                new(ClaimTypes.Email, this.Request.Email),
                new(ClaimTypes.Name, this.Request.UserName),
                //new(ApplicationClaimTypes.Depart, this.Request.DeptName.xValue()),
                //new(ApplicationClaimTypes.Level, this.Request.LvlName.xValue()),
                //new(ClaimTypes.MobilePhone, this.Request.PhoneNumber.xIsNotEmpty() ? this.Request.PhoneNumber.vToAESDecrypt() : string.Empty),
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

        this.Result = claims;
    }
}