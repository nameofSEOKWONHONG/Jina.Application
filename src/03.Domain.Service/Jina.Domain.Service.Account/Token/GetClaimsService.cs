using eXtensionSharp;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.Service.Infra.Const;
using Jina.Domain.Service.Infra.Extension;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jina.Domain.Service.Account.Token
{
	public class GetClaimsService : ServiceImplBase<GetClaimsService, Entity.Account.User, IEnumerable<Claim>>, IGetClaimsService
    {
        public GetClaimsService(AppDbContext db) : base(db, null)
        {
        }

        public override Task<bool> OnExecutingAsync()
        {
            return Task.FromResult(true);
        }

        public override async Task OnExecuteAsync()
        {
            var userClaims = new List<Claim>();
            var tenant = await DbContext.Tenants.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == Request.TenantId);
            var userRole = await DbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.UserId == Request.Id);
            var roles = await DbContext.Roles.Where(m => m.TenantId == Request.TenantId && m.Id == userRole.RoleId).ToListAsync();
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
                var results = await DbContext.RoleClaims.AsNoTracking().Where(m => m.TenantId == Request.TenantId && m.RoleId == role.Id)
                    .ToListAsync();
                var allPermissionsForThisRoles = new List<Claim>();
                results.ForEach(item =>
                {
                    allPermissionsForThisRoles.Add(new Claim(item.ClaimType, item.ClaimValue));
                });
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ApplicationClaimTypeConst.TenantId, Request.TenantId),
                new(ApplicationClaimTypeConst.TimeZone, tenant.TimeZone),
                new(ClaimTypes.NameIdentifier, Request.Id),
                new(ClaimTypes.Email, Request.Email),
                new(ClaimTypes.Name, Request.UserName),
                new(ClaimTypes.MobilePhone, Request.PhoneNumber.xIsNotEmpty() ? Request.PhoneNumber.vToAESDecrypt() : string.Empty),
            }.Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            Result = claims;
        }
    }
}