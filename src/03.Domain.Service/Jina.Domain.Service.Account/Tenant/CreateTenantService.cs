using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Jina.Base.Service;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Domain.Shared.Consts;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Account
{
    /// <summary>
    /// create tenant
    /// </summary>
    [TransactionOptions(IsolationLevel.ReadCommitted)]
    public sealed class CreateTenantService : ServiceImplBase<CreateTenantService, AppDbContext, CreateTenantRequest, IResults<bool>>,
        ICreateTenantService
    {
        private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="pipe"></param>
        /// <param name="passwordHasher"></param>
        public CreateTenantService(ILogger<CreateTenantService> logger, ISessionContext context, ServicePipeline pipe,
            IPasswordHasher<Entity.Account.User> passwordHasher) : base(logger, context, pipe)
        {
            _passwordHasher = passwordHasher;
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.TenantId.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("TenantId is empty");
                return false;
            }

            if (this.Request.Email.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("Email is empty");
                return false;
            }

            if (this.Request.Name.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("Name is empty");
                return false;
            }

            if (this.Request.UserName.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("User name is empty");
                return false;
            }

            if (this.Request.FirstName.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("First name is empty");
                return false;
            }

            if (this.Request.LastName.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("Last name is empty");
                return false;
            }

            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("Last name is empty");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var rootTenant = await this.Db.Tenants.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId);
            if (rootTenant.xIsEmpty())
            {
                rootTenant = new Tenant()
                {
                    TenantId = Request.TenantId,
                    Name = Request.Name,
                    RedirectUrl = Request.RedirectUrl,
                    TimeZone = Request.TimeZone.xValue<string>("Korea Standard Time"),
                };

                await this.Db.Tenants.AddAsync(rootTenant);
                await this.Db.SaveChangesAsync();
            }

            //Check if Role Exists
            var adminRole = new Role(Request.TenantId, RoleConstants.AdminRole, "Root role with system permissions")
            {
                IsActive = true,
                NormalizedName = $"{RoleConstants.AdminRole.ToUpper()}",
                CreatedBy = "SYSTEM",
                CreatedOn = DateTime.Now,
            };
            var adminRoleInDb = await this.Db.Roles.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Name == RoleConstants.AdminRole);
            if (adminRoleInDb.xIsEmpty())
            {
                await this.Db.Roles.AddAsync(adminRole);
                await this.Db.SaveChangesAsync();

                adminRoleInDb = await this.Db.Roles.FirstAsync(m => m.TenantId == Request.TenantId && m.Name == RoleConstants.AdminRole);
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

            var rootUserInDb = await this.Db.Users.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Email == adminUser.Email);
            if (rootUserInDb.xIsEmpty())
            {
                await this.Db.Users.AddAsync(adminUser);
                await this.Db.SaveChangesAsync();
                var userRole = new UserRole()
                {
                    TenantId = Request.TenantId,
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id,
                    CreatedBy = "SYSTEM",
                    CreatedOn = DateTime.Now,
                };

                await this.Db.UserRoles.AddAsync(userRole);
                await this.Db.SaveChangesAsync();

                foreach (var permission in Permissions.GetRegisteredPermissions())
                {
                    var result = await AddPermissionClaim(adminRoleInDb, permission);
                    if (result.Succeeded.xIsFalse())
                    {
                        Result = await Results<bool>.SuccessAsync(false);
                        return;
                    }
                }
            }

            Result = await Results<bool>.SuccessAsync(true);
        }


        #region [roleclaim]
        private string CreateSecurityStamp(Entity.Account.User user)
        {
            if (user.xIsEmpty()) throw new Exception("user is empty.");
            return $"{user.Id}{user.FirstName}{user.LastName}{user.Email}{DateTime.UtcNow}".xGetHashCode();
        }

        private async Task<IdentityResult> AddPermissionClaim(Role role, string permission)
        {
            var allClaims = await this.Db.RoleClaims.Where(m => m.TenantId == role.TenantId && m.RoleId == role.Id)
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
                await this.Db.RoleClaims.AddRangeAsync(list);
                await this.Db.SaveChangesAsync();
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }
        #endregion
    }
}

