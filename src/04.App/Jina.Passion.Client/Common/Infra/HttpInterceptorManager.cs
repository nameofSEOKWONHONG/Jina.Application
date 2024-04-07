using FluentValidation;
using Jina.Passion.Client.Services.Account;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Jina.Passion.Client.Common.Infra
{
    internal class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAccountService _accountService;
        private readonly NavigationManager _navigationManager;        

        public HttpInterceptorManager(HttpClientInterceptor interceptor,
            IAccountService accountService,
            NavigationManager navigationManager)
        {
            _interceptor = interceptor;
            _accountService = accountService;
            _navigationManager = navigationManager;                        
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
//                try
//                {
//                    var token = await _accountService.TryRefreshToken();
//                    if (!string.IsNullOrEmpty(token))
//                    {
//                        _snackBar.Add("Refreshed Token.", Severity.Success);
//                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                    }
//                }
//                catch (Exception ex)
//                {
//#if DEBUG
//                    _logger.LogError(ex, "InterceptBeforeHttpAsync Error : {Error}", ex.Message);
//#endif
//                    _snackBar.Add("You are Logged Out.", Severity.Error);
//                    await _accountService.Logout();
//                    _navigationManager.NavigateTo("/Login");
//                }
            }
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
    }
}
