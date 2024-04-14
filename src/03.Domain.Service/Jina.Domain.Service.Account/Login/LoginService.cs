using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Jina.Base.Service;

namespace Jina.Domain.Service.Account
{
	[TransactionOptions()]
	public class LoginService : ServiceImplBase<LoginService, AppDbContext, TokenRequest, IResultBase<TokenResult>>, ILoginService
	{
		private readonly IPasswordHasher<Jina.Domain.Entity.Account.User> _passwordHasher;
		private Jina.Domain.Entity.Account.User _user;
		private readonly ApplicationConfig _config;

		public LoginService(ISessionContext ctx, ServicePipeline svc,
			IPasswordHasher<Jina.Domain.Entity.Account.User> passwordHasher,
			IOptions<ApplicationConfig> options) : base(ctx, svc)
		{
			_passwordHasher = passwordHasher;
			_config = options.Value;
		}

		public override async Task OnExecutingAsync()
		{
			_user = await this.Db.Users.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Email == Request.Email);
			if (_user.xIsEmpty())
			{
				this.Result = await ResultBase<TokenResult>.FailAsync("User Not Found.");
				return;
			}
			if (!_user.IsActive)
			{
				this.Result = await ResultBase<TokenResult>.FailAsync("User Not Active. Please contact the administrator.");
				return;
			}
			if (!_user.EmailConfirmed)
			{
				this.Result = await ResultBase<TokenResult>.FailAsync("E-Mail not confirmed.");
				return;
			}
			if (_user.TenantId != Request.TenantId)
			{
				this.Result = await ResultBase<TokenResult>.FailAsync("Tenant-Id not Matched.");
				return;
			}

			var passwordValid = _passwordHasher.VerifyHashedPassword(_user, _user.PasswordHash, Request.Password);
			if (passwordValid != PasswordVerificationResult.Success)
			{
				this.Result = await ResultBase<TokenResult>.FailAsync("Invalid Credentials.");
				return;
			}
		}

		public override async Task OnExecuteAsync()
		{
			_user.RefreshToken = GenerateRefreshToken();
			_user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
			this.Db.Users.Update(_user);
			await this.Db.SaveChangesAsync();

			var token = await GenerateJwtAsync(_user);
			var response = new TokenResult { Token = token, 
				RefreshToken = _user.RefreshToken, 
				UserImageURL = _user.ProfilePictureDataUrl };

			this.Result = await ResultBase<TokenResult>.SuccessAsync(response);
		}

		#region [private method]

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private async Task<string> GenerateJwtAsync(Jina.Domain.Entity.Account.User user)
		{
			var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
			return token;
		}

		private async Task<IEnumerable<Claim>> GetClaimsAsync(Jina.Domain.Entity.Account.User user)
		{
			var userClaims = new List<Claim>();
			var tenant = await this.Db.Tenants.FirstOrDefaultAsync(m => m.TenantId == user.TenantId);
			var userRole = await this.Db.UserRoles.FirstOrDefaultAsync(m => m.TenantId == user.TenantId && m.UserId == user.Id);
			var roles = await this.Db.Roles.Where(m => m.TenantId == user.TenantId && m.Id == userRole.RoleId).ToListAsync();
			var roleClaims = new List<Claim>();
			var permissionClaims = new List<Claim>();

			foreach (var role in roles)
			{
				roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
				var results = await this.Db.RoleClaims.Where(m => m.TenantId == user.TenantId && m.RoleId == role.Id)
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
				new(ApplicationClaimTypes.TenantId, user.TenantId),
				//new(ApplicationClaimTypes.TimeZone, tenant.TimeZone),
				new(ClaimTypes.NameIdentifier, user.Id),
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.Name, user.UserName),
				//new(ApplicationClaimTypes.Depart, user.DeptName.xValue()),
				//new(ApplicationClaimTypes.Level, user.LvlName.xValue()),
				//new(ClaimTypes.MobilePhone, user.PhoneNumber.xIsNotEmpty() ? user.PhoneNumber.vToAESDecrypt() : string.Empty),
			}
			.Union(userClaims)
			.Union(roleClaims)
			.Union(permissionClaims);

			return claims;
		}

		private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
		{
			var token = new JwtSecurityToken(
			   claims: claims,
			   expires: DateTime.UtcNow.AddDays(1),
			   signingCredentials: signingCredentials);
			var tokenHandler = new JwtSecurityTokenHandler();
			var encryptedToken = tokenHandler.WriteToken(token);
			return encryptedToken;
		}

		private SigningCredentials GetSigningCredentials()
		{
			var secret = Encoding.UTF8.GetBytes(_config.Secret);
			return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
		}

		#endregion
	}
}
