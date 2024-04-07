using AntDesign;
using AntDesign.ProLayout;
using Blazored.SessionStorage;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel.Consts;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Infra;
using Jina.Passion.Client.Layout.ViewModels;
using Jina.Passion.Client.Pages.Account.Services;
using Jina.Passion.Client.Pages.Account.ViewModels;
using Jina.Passion.Client.Pages.Weather.Services;
using Jina.Passion.Client.Pages.Weather.ViewModels;
using Jina.Passion.Client.Services;
using Jina.Passion.Client.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Jina.Passion.Client
{
    public class Program
    {
        private const string ClientName = "Jina.Passion.API";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            #region [http 설정]

            builder.Services.AddTransient<IHttpInterceptorManager, HttpInterceptorManager>();
            builder.Services.AddTransient<AuthenticationHeaderHandler>();
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(ClientName)
                .EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    var culture = CultureInfo.DefaultThreadCurrentCulture;
                    if(culture.xIsEmpty())
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd($"{culture.ToString()},{culture.Name}");
                    client.BaseAddress = new Uri("https://localhost:7103");
                })
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            builder.Services.AddHttpClientInterceptor();

            #endregion [http 설정]

            builder.Services.AddAntDesign();
            //builder.Services.Configure<ProSettings>(x =>
            //{
            //    x.Title = "Ant Design Pro";
            //    x.NavTheme = "realDark";
            //    x.HeaderHeight = 48;
            //    x.Layout = "side";
            //    x.ContentWidth = "Fluid";
            //    x.FixedHeader = false;
            //    x.FixSiderbar = true;
            //    x.Title = "Ant Design Pro";
            //    x.IconfontUrl = null;
            //    x.PrimaryColor = "cyan";
            //    x.ColorWeak = false;
            //    x.SplitMenus = false;
            //    x.HeaderRender = true;
            //    x.FooterRender = true;
            //    x.MenuRender = true;
            //    x.MenuHeaderRender = true;
            //});
            builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<WeatherService>();
            builder.Services.AddScoped<WeatherViewModel>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserListViewModel>();
            builder.Services.AddScoped<MenuRoleViewModel>();
            builder.Services.AddScoped<NotificationViewModel>();
            builder.Services.AddScoped<IChartService, ChartService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddAuthorizationCore(options =>
            {
                RegisterPermissionClaims(options);
            });

            builder.Services.AddSingleton<IRestClient, RestClient>();
            builder.Services.AddSingleton<AuthenticationStateProviderImpl>();
            builder.Services.AddSingleton<AuthenticationStateProvider, AuthenticationStateProviderImpl>();
            builder.Services.AddSingleton<ISessionStorageHandler, SessionStorageHandler>();
            builder.Services.AddBlazoredSessionStorageAsSingleton();

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var build = builder.Build();
            var vm = build.Services.GetRequiredService<NotificationViewModel>();
            await vm.InitializeAsync();

            await InitializeCultureAsync(build);

            await build.RunAsync();
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(PermissionConsts).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.xValue<string>(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.xValue<string>()));
                }
            }
        }

        private static async Task InitializeCultureAsync(WebAssemblyHost host)
        {
            var handler = host.Services.GetRequiredService<ISessionStorageHandler>();
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
        }
    }
}