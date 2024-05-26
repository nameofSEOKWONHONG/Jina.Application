using eXtensionSharp;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using Hangfire;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net;
using Jina.Domain.Net;
using Jina.Domain.Service.Net;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Domain.Shared.Consts;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Account.User
{
	public sealed class RegisterUserService : ServiceImplBase<RegisterUserService, AppDbContext, RegisterRequest, IResults<string>>, 
        IRegisterUserService        
    {
        private readonly IPasswordHasher<Entity.Account.User> _passwordHasher;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="pipe"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="emailService"></param>
        public RegisterUserService(ILogger<RegisterUserService> logger, ISessionContext context, ServicePipeline pipe,
            IPasswordHasher<Entity.Account.User> passwordHasher) : base(logger, context, pipe)
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
                this.Result = await Results<string>.FailAsync("Email already exist");
                return false;
            }

            var phone = await this.Db.Users.AnyAsync(m =>
                m.TenantId == this.Request.TenantId &&
                m.PhoneNumber == this.Request.PhoneNumber.vToAESEncrypt());

            if (phone.xIsNotEmpty())
            {
                this.Result = await Results<string>.FailAsync("Phone already exist");
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
                this.Result = await Results<string>.FailAsync("Email already taken.");
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
                    this.Result = await Results<string>.FailAsync("Phone number already registered.");
                    return;
                }
            }

            var userWithSameEmail = await this.Db.Users
                .FirstOrDefaultAsync(m => m.TenantId == this.Request.TenantId &&
                m.Email == this.Request.Email);
            if (userWithSameEmail.xIsNotEmpty())
            {
                this.Result = await Results<string>.FailAsync($"User Create Failed.");
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

                if (!this.Request.AutoConfirmEmail)
                {
                    if (this.Request.Email.xIsEmail().xIsFalse()) throw new Exception("Not correct email address");

                    //TODO : 메일 컨펌을 위한 사이트 URL 확인
                    var verificationUri = "test.com/emailconfirm?request=sadijoijsaiojiosdf";//await SendVerificationEmail(user, origin);
                    var mailRequest = new EmailRequest
                    {
                        FromMailers = new List<MailSender>()
                        {
                            new MailSender("admin", "admin@test.com")
                        },
                        ToMailers = new List<MailSender>()
                        {
                            new MailSender("", "")
                        },
                        Body = $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>.",
                        Subject = "Confirm Registration"
                    };
                    this.Context.JobClient.Enqueue<EmailJob>(m => m.ExecuteAsync(mailRequest));
                    
                    this.Result = await Results<string>.SuccessAsync(user.Id,
                        $"User {user.UserName} Registered. Please check your Mailbox to verify!");
                    return;
                }

                this.Result = await Results<string>
                    .SuccessAsync("User Registered.");
            }
        }
    }
}