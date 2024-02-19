using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Account.User;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.Extension;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Domain.SharedKernel.Consts;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace Jina.Domain.Service.Account.User
{
    public class RegisterUserService : EfServiceImpl<RegisterUserService, RegisterRequest, IResultBase<bool>>, IRegisterUserService        
    {
        private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;

        public RegisterUserService(AppDbContext db,
            IPasswordHasher<Entity.Account.User> passwordHasher) : base(db, null)
        {
            _passwordHasher = passwordHasher;
        }

        public override async Task<bool> OnExecutingAsync()
        {
            var user = await this.DbContext.Users.AnyAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);

            if (user.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync("Email already exist");
                return false;
            }

            var phone = await this.DbContext.Users.AnyAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.PhoneNumber == this.Request.PhoneNumber.vToAESEncrypt());

            if (phone.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync("Phone already exist");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var userWithSameUserName = await this.DbContext.Users.FirstOrDefaultAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);

            if (userWithSameUserName.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync("Email already taken.");
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
                var userWithSamePhoneNumber = await this.DbContext.Users
                    .FirstOrDefaultAsync(x => x.TenantId == this.Request.TenantId &&
                        x.PhoneNumber == this.Request.PhoneNumber.vToAESEncrypt());

                if (userWithSamePhoneNumber.xIsNotEmpty())
                {
                    this.Result = await Result<bool>.FailAsync("Phone number already registered.");
                    return;
                }
            }

            var userWithSameEmail = await this.DbContext.Users
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);
            if (userWithSameEmail.xIsNotEmpty())
            {
                this.Result = await Result<bool>.FailAsync($"User Create Failed.");
                return;
            }

            var hashedPassword = _passwordHasher.HashPassword(user, this.Request.Password);
            user.PasswordHash = hashedPassword;
            user.SecurityStamp = $"{user.Id}{user.FirstName}{user.LastName}{user.Email}{DateTime.Now}".xGetHashCode();
            await this.DbContext.Users.AddAsync(user);
            await this.DbContext.SaveChangesAsync();

            if (user.Id.xIsNotEmpty())
            {
                var basicRole = await this.DbContext.Roles
                    .FirstAsync(m => m.TenantId == this.Request.TenantId &&
                    m.Name == RoleConst.BasicRole);

                await this.DbContext.UserRoles.AddAsync(new UserRole()
                {
                    TenantId = this.Request.TenantId,
                    UserId = user.Id,
                    RoleId = basicRole.Id,
                });
                await this.DbContext.SaveChangesAsync();

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

                this.Result = await Result<bool>
                    .SuccessAsync("User Registered.");
            }
        }
    }
}