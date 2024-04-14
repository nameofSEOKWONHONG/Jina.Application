using AntDesign;
using eXtensionSharp;
using Jina.Domain.Account.Token;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Infra;
using Jina.Passion.Client.Services.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Jina.Passion.Client.Pages.User
{
    public partial class Login
    {
        private readonly TokenRequest _model = new TokenRequest();

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IAccountService AccountService { get; set; }

        [Inject] public MessageService Message { get; set; }

        [Inject] public AuthenticationStateProviderImpl AuthenticationStateProviderImpl { get; set; }
        [Inject] public ISessionStorageHandler BrowserStorageHandler { get; set; }        

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProviderImpl.GetAuthenticationStateAsync();
            if(authState != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                NavigationManager.NavigateTo("/");
            }

            _model.RemamberMe = await BrowserStorageHandler.GetAsync<bool>(nameof(TokenRequest.RemamberMe));
            _model.TenantId = await BrowserStorageHandler.GetAsync(nameof(TokenRequest.TenantId));
            _model.Email = await BrowserStorageHandler.GetAsync(nameof(TokenRequest.Email));
        }


        public async Task HandleSubmit()
        {
            var result = await AccountService.Login(_model);
            if (!result.Succeeded)
            {
                var message = result.Messages.xJoin();
                await Message.Error(message);
                return;
            }

            //if (_model.RemamberMe == true)
            //{
            //    await BrowserStorageHandler.SetAsync<bool>(nameof(TokenRequest.RemamberMe), _model.RemamberMe);
            //    await BrowserStorageHandler.SetAsync(nameof(TokenRequest.TenantId), _model.TenantId);
            //    await BrowserStorageHandler.SetAsync(nameof(TokenRequest.Email), _model.Email);
            //}
        }

        public async Task GetCaptcha()
        {
            //var captcha = await AccountService.GetCaptchaAsync(_model.Mobile);
            //await Message.Success($"Verification code validated successfully! The verification code is: {captcha}");
        }
    }
}
