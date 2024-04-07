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

namespace Jina.Domain.Service.Account.Login
{
	[TransactionOptions(System.Transactions.TransactionScopeOption.Required)]
	public class LoginService : ServiceImplBase<LoginService, TokenRequest, IResultBase<TokenResult>>, ILoginService
	{
		private readonly IPasswordHasher<Jina.Domain.Entity.Account.User> _passwordHasher;
		private Jina.Domain.Entity.Account.User _user;
		private readonly ApplicationConfig _config;

		public LoginService(AppDbContext db, 
			ISessionContext context, 
			IPasswordHasher<Jina.Domain.Entity.Account.User> passwordHasher,
			IOptions<ApplicationConfig> options) : base(db, context)
		{
			_passwordHasher = passwordHasher;
			_config = options.Value;
		}

		public override async Task OnExecutingAsync()
		{
			_user = await this.DbContext.Users.FirstOrDefaultAsync(m => m.TenantId == Request.TenantId && m.Email == Request.Email);
			if (_user.xIsEmpty())
			{
				this.Result = await Result<TokenResult>.FailAsync("User Not Found.");
			}
			if (!_user.IsActive)
			{
				this.Result = await Result<TokenResult>.FailAsync("User Not Active. Please contact the administrator.");
			}
			if (!_user.EmailConfirmed)
			{
				this.Result = await Result<TokenResult>.FailAsync("E-Mail not confirmed.");
			}
			if (_user.TenantId != Request.TenantId)
			{
				this.Result = await Result<TokenResult>.FailAsync("Tenant-Id not Matched.");
			}

			var passwordValid = _passwordHasher.VerifyHashedPassword(_user, _user.PasswordHash, Request.Password);
			if (passwordValid != PasswordVerificationResult.Success)
			{
				this.Result = await Result<TokenResult>.FailAsync("Invalid Credentials.");				
			}
		}

		public override async Task OnExecuteAsync()
		{
			_user.RefreshToken = GenerateRefreshToken();
			_user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
			this.DbContext.Users.Update(_user);
			await this.DbContext.SaveChangesAsync();

			var token = await GenerateJwtAsync(_user);
			var response = new TokenResult { Token = token, 
				RefreshToken = _user.RefreshToken, 
				UserImageURL = _user.ProfilePictureDataUrl };

			this.Result = await Result<TokenResult>.SuccessAsync(response);
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
			var tenant = await this.DbContext.Tenants.FirstOrDefaultAsync(m => m.TenantId == user.TenantId);
			var userRole = await this.DbContext.UserRoles.FirstOrDefaultAsync(m => m.TenantId == user.TenantId && m.UserId == user.Id);
			var roles = await this.DbContext.Roles.Where(m => m.TenantId == user.TenantId && m.Id == userRole.RoleId).ToListAsync();
			var roleClaims = new List<Claim>();
			var permissionClaims = new List<Claim>();

			foreach (var role in roles)
			{
				roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
				var results = await this.DbContext.RoleClaims.Where(m => m.TenantId == user.TenantId && m.RoleId == role.Id)
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
