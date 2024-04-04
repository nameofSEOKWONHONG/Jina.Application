using eXtensionSharp;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.Service.Infra.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jina.Domain.Service.Account.Token
{
	public class GetSigningCredentialsService : ServiceImplBase<GetSigningCredentialsService, bool, SigningCredentials>, IGetSigningCredentialsService
    {
        private readonly IConfiguration _configuration;

        public GetSigningCredentialsService(AppDbContext db, IConfiguration configuration) : base(db, null)
        {
            _configuration = configuration;
        }

        public override Task<bool> OnExecutingAsync()
        {
            return Task.FromResult(true);
        }

        public override Task OnExecuteAsync()
        {
            var secret = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Application:Secret").xToSHA512Decrypt(ApplicationConsts.Encryption.DB_ENC_SHA512_KEY));
            Result = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
            return Task.CompletedTask;
        }
    }
}