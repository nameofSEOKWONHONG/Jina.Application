using Jina.Passion.Client.Services.Account;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using AntDesign;
using Jina.Passion.Client.Base.Abstract;
using Toolbelt.Blazor;
using ILogger = Serilog.ILogger;

namespace Jina.Passion.Client.Common.Infra
{
    internal class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAccountService _accountService;
        private readonly NavigationManager _navigationManager;
        private readonly IMessageService _messageService;
        private readonly ILogger _logger = Serilog.Log.ForContext<HttpInterceptorManager>();
        
        public HttpInterceptorManager(HttpClientInterceptor interceptor,
            IAccountService accountService,
            NavigationManager navigationManager,
            IMessageService messageService)
        {
            _interceptor = interceptor;
            _accountService = accountService;
            _navigationManager = navigationManager;
            _messageService = messageService;
        }

        public void DisposeEvent()
        {
            _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
            _interceptor.AfterSendAsync -= InterceptAfterHttpAsync;
        }

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
                try
                {
                    var token = await _accountService.TryRefreshToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        await _messageService.Success("Refreshed Token.");
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "InterceptBeforeHttpAsync:{msg}", ex.Message);
                    await _messageService.Error("You are Logged Out.");
                    await Task.Delay(1000);
                    await _accountService.Logout();
                    _navigationManager.NavigateTo("/Login");
                }
            }
        }

        private Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void RegisterEvent()
        {
            _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
            _interceptor.AfterSendAsync += InterceptAfterHttpAsync;
        }
    }
}
