using eXtensionSharp;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra.Base;
using System.Security.Cryptography;

namespace Jina.Domain.Service.Account.Token
{
	public class GetRefreshTokenService : ServiceImplBase<GetRefreshTokenService, Entity.Account.User, string>, IGetRefreshTokenService
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