using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using System.Data.Entity;

namespace Jina.Domain.Service.Account.User
{
	public class CreateRoleService : ServiceImplBase<CreateRoleService, CreateRoleRequest, IResultBase<bool>>
        , IScopeService
    {
        public CreateRoleService(AppDbContext db) : base(db, null)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            var id = await this.DbContext.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Id == this.Request.Id);

            if (id.xIsEmpty())
            {
                this.Result = await Result<bool>.FailAsync("already exist");
                return false;
            }

            var name = await this.DbContext.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Name == this.Request.Name);

            if (name.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync("already exist");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var existingRole = await this.DbContext.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Name == this.Request.Name);

            var role = new Role()
            {
                TenantId = this.Request.TenantId,
                Name = this.Request.Name,
                Description = this.Request.Description,
                NormalizedName = $"{this.Request.TenantId}_{this.Request.Name.ToUpper()}",
            };
            await this.DbContext.Roles.AddAsync(role);
            await this.DbContext.SaveChangesAsync();
        }
    }

    public class CreateRoleClaimService : ServiceImplBase<CreateRoleClaimService, CreateRoleClaimRequest, IResultBase<bool>>
        , IScopeService
    {
        public CreateRoleClaimService(AppDbContext db) : base(db, null)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            var exist = await this.DbContext.RoleClaims
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                    m.Id == this.Request.Id &&
                    m.RoleId == this.Request.RoleId &&
                    m.ClaimType == this.Request.Type &&
                    m.ClaimValue == this.Request.Value);
            if (exist.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync("already exists");
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
            await this.DbContext.RoleClaims.AddAsync(roleClaim);
            await this.DbContext.SaveChangesAsync();
            this.Result = await Result<bool>.SuccessAsync("Role Claim created.");
        }
    }
}