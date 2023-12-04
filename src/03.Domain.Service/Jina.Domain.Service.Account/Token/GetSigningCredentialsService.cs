using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Service.Account.Token
{
    public class GetSigningCredentialsService : EfServiceImpl<GetSigningCredentialsService, bool, SigningCredentials>, IGetSigningCredentialsService
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