using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using System.Data.Entity;
using Jina.Base.Service;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Account.User
{
	public sealed class CreateRoleService : ServiceImplBase<CreateRoleService, AppDbContext, CreateRoleRequest, IResults<bool>>
        , IScopeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="svc"></param>
        public CreateRoleService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task OnExecutingAsync()
        {
            var id = await this.Db.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Id == this.Request.Id);

            if (id.xIsEmpty())
            {
                this.Result = await Results<bool>.FailAsync("already exist");
                return;
            }

            var name = await this.Db.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Name == this.Request.Name);

            if (name.xIsNotEmpty())
            {
                this.Result = await Results<bool>.FailAsync("already exist");
                return;
            }
        }

        public override async Task OnExecuteAsync()
        {
            var existingRole = await this.Db.Roles
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Name == this.Request.Name);

            var role = new Role()
            {
                TenantId = this.Request.TenantId,
                Name = this.Request.Name,
                Description = this.Request.Description,
                NormalizedName = $"{this.Request.TenantId}_{this.Request.Name.ToUpper()}",
            };
            await this.Db.Roles.AddAsync(role);
            await this.Db.SaveChangesAsync();
        }
    }
}