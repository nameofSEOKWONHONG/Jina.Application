using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Account;
using Jina.Domain.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Service.Account.Token
{
    public class GetRefreshTokenService : DomainServiceImpl<GetRefreshTokenService, User, string>, IGetRefreshTokenService
    {
        public GetRefreshTokenService(AppDbContext db) : base(db, null)
        {
        }

        public override Task<bool> OnExecutingAsync()
        {
            if (Request.xIsEmpty()) Task.FromResult(false);

            return Task.FromResult(true);
        }

        public override Task OnExecuteAsync()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            Result = refreshToken;
            return Task.CompletedTask;
        }
    }
}