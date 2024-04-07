using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Domain.SharedKernel.Consts;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Transactions;

namespace Jina.Domain.Service.Account
{
    /// <summary>
    /// create tenant
    /// </summary>
	[TransactionOptions(TransactionScopeOption.Required)]
    public class CreateTenantService : ServiceImplBase<CreateTenantService, CreateTenantRequest, IResultBase<bool>>,
        ICreateTenantService
    {
        private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;

        public CreateTenantService(AppDbContext db, ISessionContext context,
            IPasswordHasher<Entity.Account.User> passwordHasher) : base(db, context)
        {
            _passwordHasher = passwordHasher;
        }

        public override Task OnExecutingAsync()
        {
            return Task.CompletedTask;
        }

        public override async Task OnExecuteAsync()
        {
            var rootTenant = await DbContext.Tenants.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId);
            if (rootTenant.xIsEmpty())
            {
                rootTenant = new Tenant()
                {
                    TenantId = Request.TenantId,
                    Name = Request.Name,
                    RedirectUrl = Request.RedirectUrl,
                    TimeZone = Request.TimeZone.xValue("Korea Standard Time"),
                };

                await DbContext.Tenants.AddAsync(rootTenant);
                await DbContext.SaveChangesAsync();
            }

            //Check if Role Exists
            var adminRole = new Role(Request.TenantId, RoleConstants.AdminRole, "Root role with system permissions")
            {
                IsActive = true,
                NormalizedName = $"{RoleConstants.AdminRole.ToUpper()}",
                CreatedBy = "SYSTEM",
                CreatedOn = DateTime.Now,
            };
            var adminRoleInDb = await DbContext.Roles.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Name == RoleConstants.AdminRole);
            if (adminRoleInDb.xIsEmpty())
            {
                await DbContext.Roles.AddAsync(adminRole);
                await DbContext.SaveChangesAsync();

                adminRoleInDb = await DbContext.Roles.FirstAsync(m => m.TenantId == Request.TenantId && m.Name == RoleConstants.AdminRole);
            }

            //Check if User Exists
            var adminUser = new Entity.Account.User
            {
                TenantId = Request.TenantId,
                Id = "admin",
                FirstName = this.Request.FirstName,
                LastName = this.Request.LastName,
                Email = this.Request.Email,
                UserName = this.Request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedBy = "SYSTEM",
                CreatedOn = DateTime.Now,
                IsActive = true,
            };

            var passwordHash = _passwordHasher.HashPassword(adminUser, RoleConstants.AdminPassword);
            adminUser.PasswordHash = passwordHash;
            adminUser.SecurityStamp = CreateSecurityStamp(adminUser);

            var rootUserInDb = await DbContext.Users.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Email == adminUser.Email);
            if (rootUserInDb.xIsEmpty())
            {
                await DbContext.Users.AddAsync(adminUser);
                await DbContext.SaveChangesAsync();
                var userRole = new UserRole()
                {
                    TenantId = Request.TenantId,
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id,
                    CreatedBy = "SYSTEM",
                    CreatedOn = DateTime.Now,
                };

                await DbContext.UserRoles.AddAsync(userRole);
                await DbContext.SaveChangesAsync();

                foreach (var permission in PermissionConsts.GetRegisteredPermissions())
                {
                    var result = await AddPermissionClaim(adminRoleInDb, permission);
                    if (result.Succeeded.xIsFalse())
                    {
                        Result = await Result<bool>.SuccessAsync(false);
                        return;
                    }
                }
            }

            Result = await Result<bool>.SuccessAsync(true);
        }


        #region [roleclaim]
        private string CreateSecurityStamp(Entity.Account.User user)
        {
            if (user.xIsEmpty()) throw new Exception("user is empty.");
            return $"{user.Id}{user.FirstName}{user.LastName}{user.Email}{DateTime.UtcNow}".xGetHashCode();
        }

        private async Task<IdentityResult> AddPermissionClaim(Role role, string permission)
        {
            var allClaims = await DbContext.RoleClaims.Where(m => m.TenantId == role.TenantId && m.RoleId == role.Id)
                .ToListAsync();
            var list = new List<RoleClaim>();
            if (!allClaims.Any(a => a.ClaimType == ApplicationClaimTypes.Permission && a.ClaimValue == permission))
            {
                var claim = new Claim(ApplicationClaimTypes.Permission, permission);
                list.Add(new RoleClaim()
                {
                    TenantId = role.TenantId,
                    RoleId = role.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                    CreatedBy = role.CreatedBy,
                    CreatedOn = role.CreatedOn,
                });
            }

            if (list.xIsNotEmpty())
            {
                await DbContext.RoleClaims.AddRangeAsync(list);
                await DbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }
        #endregion
    }
}
