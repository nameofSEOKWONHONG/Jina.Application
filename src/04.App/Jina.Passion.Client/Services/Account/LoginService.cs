using System.Net.Http.Headers;
using eXtensionSharp;
using Jina.Domain.Account.Token;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Consts;
using Jina.Passion.Client.Common.Infra;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jina.Passion.Client.Services.Account
{
    public class AccountService : ServiceBase, IAccountService
    {
        /// <summary>
        /// 계정 서비스
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionStorageService"></param>
        /// <param name="authenticationStateProvider"></param>
        public AccountService(IRestClient client, 
            ISessionStorageService sessionStorageService, 
            AuthenticationStateProvider authenticationStateProvider) : base(client, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<IResults<TokenResult>> Login(TokenRequest request)
        {   
            var result = await this.Client.ExecuteAsync<TokenRequest, Results<TokenResult>>(HttpMethod.Post, 
                "api/account/login", request);
            if (result.Succeeded)
            {
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;
                var userImageURL = result.Data.UserImageURL;

                await this.SessionStorageService.SetAsync(StorageConsts.Local.AuthToken, token);
                await this.SessionStorageService.SetAsync(StorageConsts.Local.RefreshToken, refreshToken);
                await ((AuthenticationStateProviderImpl)AuthenticationStateProvider).StateChangedAsync();

                return result;
            }

            return await Results<TokenResult>.FailAsync(result.Messages);
        }
        
        public async Task<string> TryRefreshToken()
        {
            //check if token exists
            var availableToken = await SessionStorageService.GetAsync(StorageConsts.Local.RefreshToken);
            if (availableToken.xIsEmpty()) return string.Empty;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<IResults> Logout()
        {
            var removeTargets = new[]
            {
                StorageConsts.Local.AuthToken,
                StorageConsts.Local.RefreshToken,
                StorageConsts.Local.UserImageURL,
                // StorageConsts.Local.SelectedMenuGroup,
                // StorageConsts.Local.SelectedSubMenu
            };

            await SessionStorageService.RemoveAllAsync(removeTargets);

            ((AuthenticationStateProviderImpl)AuthenticationStateProvider).MarkUserAsLoggedOut();
            this.Client.HttpClient.DefaultRequestHeaders.Authorization = null;
            return await Results.SuccessAsync();
        }

        private async Task<string> RefreshToken()
        {
            var token = await SessionStorageService.GetAsync(StorageConsts.Local.AuthToken);
            var refreshToken = await SessionStorageService.GetAsync(StorageConsts.Local.RefreshToken);
            var request = new RefreshTokenRequest()
            {
                Token = token,
                RefreshToken = refreshToken
            };
            
            var result = await this.Client.ExecuteAsync<RefreshTokenRequest, Results<TokenResult>>(HttpMethod.Post, 
                "api/account/refresh", request);
            
            if (!result.Succeeded)
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }
            
            if (result.Succeeded)
            {
                token = result.Data.Token;
                refreshToken = result.Data.RefreshToken;
                //var userImageURL = result.Data.UserImageURL;

                await SessionStorageService.SetAsync(StorageConsts.Local.AuthToken, token);
                await SessionStorageService.SetAsync(StorageConsts.Local.RefreshToken, refreshToken);
                this.Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return token;
            }

            return string.Empty;
        }
    }
}
