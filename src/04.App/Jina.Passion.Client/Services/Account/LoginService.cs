using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Consts;
using Jina.Passion.Client.Common.Infra;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Jina.Passion.Client.Services.Account
{
    public interface IAccountService
    {
        Task<IResultBase<TokenResult>> Login(TokenRequest model);
    }

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly ISessionStorageHandler _sessionStorageHandler;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AccountService(IRestClient client, ISessionStorageHandler sessionStorageHandler, AuthenticationStateProvider authenticationStateProvider) : base(client)
        {
            _sessionStorageHandler = sessionStorageHandler;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IResultBase<TokenResult>> Login(TokenRequest model)
        {   
            var result = await this.Client.ExecuteAsync<TokenRequest, Result<TokenResult>>(HttpMethod.Post, 
                "api/login/login", model);
            if (result.Succeeded)
            {
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;
                var userImageURL = result.Data.UserImageURL;

                await this._sessionStorageHandler.SetAsync(StorageConsts.Local.AuthToken, token);
                await ((AuthenticationStateProviderImpl)_authenticationStateProvider).StateChangedAsync();

                return result;
            }

            return await Result<TokenResult>.FailAsync(result.Messages);
        }
    }
}
