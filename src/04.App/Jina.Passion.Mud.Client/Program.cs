using System.Globalization;
using System.Reflection;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared.Consts;
using Jina.Lang.Abstract;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Services.Account;
using Jina.Passion.Client.Share.Common;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Jina.Passion.Mud.Client;
using Jina.Passion.Mud.Client.Features.Account.Services;
using Jina.Passion.Mud.Client.Features.Http;
using Jina.Passion.Mud.Client.Features.Http.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("local"))
        .AddHttpClient("local", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        });

#region [http 설정]
builder.Services.AddSingleton<IHttpInterceptorManager, HttpInterceptorManager>();
builder.Services.AddSingleton<AuthenticationHeaderHandler>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("apiservice")
        .EnableIntercept(sp))
    .AddHttpClient("apiservice", client =>
    {
        var culture = CultureInfo.DefaultThreadCurrentCulture;                    
        client.DefaultRequestHeaders.AcceptLanguage.Clear();
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd($"{culture.ToString()},{culture.Name}");
        client.BaseAddress = new Uri("https://localhost:7103");
    })
    .AddHttpMessageHandler<AuthenticationHeaderHandler>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddSingleton<IRestClient, RestClient>();
#endregion [http 설정]

#region [인증 설정]
builder.Services.AddAuthorizationCore(RegisterPermissionClaims);
builder.Services.AddSingleton<AuthenticationStateProviderImpl>();
builder.Services.AddSingleton<AuthenticationStateProvider, AuthenticationStateProviderImpl>();
builder.Services.AddSingleton<IAccountService, AccountService>();
#endregion

builder.Services.AddSingleton<ILocalizer, BlazorLocalizer>();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddSingleton<ICultureInfoViewModel, CultureInfoViewModel>();
builder.Services.AddSingleton<ISessionStorageWrapperService, SessionStorageWrapperService>();
builder.Services.AddSingleton<ILocalStorageWrapperService, LocalStorageWrapperService>();


var host = builder.Build();
await InitializeCultureAsync(host);
await host.RunAsync();

static void RegisterPermissionClaims(AuthorizationOptions options)
{
    foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
    {
        var propertyValue = prop.GetValue(null);
        if (propertyValue is not null)
        {
            options.AddPolicy(propertyValue.xValue<string>(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.xValue<string>()));
        }
    }
}

static async Task InitializeCultureAsync(WebAssemblyHost host)
{
    var handler = host.Services.GetRequiredService<ISessionStorageWrapperService>();
    var result = await handler.GetAsync(ApplicationConsts.Client.CULTURE_NAME);

    CultureInfo initCulture;
    if (!result.xIsEmpty())
    {
        initCulture = new CultureInfo(result);
    }
    else
    {
        //default culture
        initCulture = new CultureInfo("en-US");
        await handler.SetAsync(ApplicationConsts.Client.CULTURE_NAME, "en-US");
    }

    CultureInfo.DefaultThreadCurrentCulture = initCulture;
    CultureInfo.DefaultThreadCurrentUICulture = initCulture;

    var service = host.Services.GetService<ILocalizer>();
    await service.xAs<BlazorLocalizer>().InitializeAsync();
}