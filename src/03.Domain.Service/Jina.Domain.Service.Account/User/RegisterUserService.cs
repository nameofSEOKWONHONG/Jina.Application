using eXtensionSharp;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Domain.SharedKernel.Consts;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using Jina.Base.Service;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Account.User
{
	public class RegisterUserService : ServiceImplBase<RegisterUserService, AppDbContext, RegisterRequest, IResultBase<bool>>, 
        IRegisterUserService        
    {
        private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;

        public RegisterUserService(ISessionContext ctx, ServicePipeline svc,
            IPasswordHasher<Entity.Account.User> passwordHasher) : base(ctx, svc)
        {
            _passwordHasher = passwordHasher;
        }

        public override async Task<bool> OnExecutingAsync()
        {
            var user = await this.Db.Users.AnyAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);

            if (user.xIsNotEmpty())
            {
                this.Result = await ResultBase<bool>.FailAsync("Email already exist");
                return false;
            }

            var phone = await this.Db.Users.AnyAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.PhoneNumber == this.Request.PhoneNumber.vToAESEncrypt());

            if (phone.xIsNotEmpty())
            {
                this.Result = await ResultBase<bool>.FailAsync("Phone already exist");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var userWithSameUserName = await this.Db.Users.FirstOrDefaultAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);

            if (userWithSameUserName.xIsNotEmpty())
            {
                this.Result = await ResultBase<bool>.FailAsync("Email already taken.");
                return;
            }

            var user = new Entity.Account.User
            {
                TenantId = this.Request.TenantId,
                Email = this.Request.Email,
                FirstName = this.Request.FirstName,
                LastName = this.Request.LastName,
                UserName = this.Request.UserName,
                PhoneNumber = this.Request.PhoneNumber.vToAESEncrypt(),
                IsActive = true,
                EmailConfirmed = true,
            };

            if (this.Request.PhoneNumber.xIsNotEmpty())
            {
                var userWithSamePhoneNumber = await this.Db.Users
                    .FirstOrDefaultAsync(x => x.TenantId == this.Request.TenantId &&
                        x.PhoneNumber == this.Request.PhoneNumber.vToAESEncrypt());

                if (userWithSamePhoneNumber.xIsNotEmpty())
                {
                    this.Result = await ResultBase<bool>.FailAsync("Phone number already registered.");
                    return;
                }
            }

            var userWithSameEmail = await this.Db.Users
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);
            if (userWithSameEmail.xIsNotEmpty())
            {
                this.Result = await ResultBase<bool>.FailAsync($"User Create Failed.");
                return;
            }

            var hashedPassword = _passwordHasher.HashPassword(user, this.Request.Password);
            user.PasswordHash = hashedPassword;
            user.SecurityStamp = $"{user.Id}{user.FirstName}{user.LastName}{user.Email}{DateTime.Now}".xGetHashCode();
            await this.Db.Users.AddAsync(user);
            await this.Db.SaveChangesAsync();

            if (user.Id.xIsNotEmpty())
            {
                var basicRole = await this.Db.Roles
                    .FirstAsync(m => m.TenantId == this.Request.TenantId &&
                    m.Name == RoleConstants.NormalRole);

                await this.Db.UserRoles.AddAsync(new UserRole()
                {
                    TenantId = this.Request.TenantId,
                    UserId = user.Id,
                    RoleId = basicRole.Id,
                });
                await this.Db.SaveChangesAsync();

                //if (!this.Request.AutoConfirmEmail)
                //{
                //    //TODO : 구현되어야 함.
                //    //var verificationUri = await SendVerificationEmail(user, origin);
                //    //var mailRequest = new MailRequest
                //    //{
                //    //    From = "mail@codewithmukesh.com",
                //    //    To = user.Email,
                //    //    Body = string.Format("Please confirm your account by <a href='{0}'>clicking here</a>.", verificationUri),
                //    //    Subject = _localizer["Confirm Registration"]
                //    //};
                //    //BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                //    return await Result<string>.SuccessAsync(user.Id, string.Format("User {0} Registered. Please check your Mailbox to verify!", user.UserName));
                //}

                this.Result = await ResultBase<bool>
                    .SuccessAsync("User Registered.");
            }
        }
    }
}