using AntDesign;
using AntDesign.ProLayout;
using Blazored.SessionStorage;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
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
using Blazored.LocalStorage;
using Jina.Domain.Shared.Consts;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using ISessionStorageService = Jina.Passion.Client.Base.Abstract.ISessionStorageService;

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
            
            #region [logging]

            var levelSwitch = new LoggingLevelSwitch();
#if DEBUG
            levelSwitch.MinimumLevel = LogEventLevel.Debug;
#else
            levelSwitch.MinimumLevel = LogEventLevel.Information;
#endif
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
                .WriteTo.BrowserHttp(endpointUrl: $"{builder.HostEnvironment.BaseAddress}ingest", controlLevelSwitch: levelSwitch)
                .CreateLogger();            

            #endregion

            
            #region [antblazor 설정]
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
            #endregion

            var isHttps = builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https";

            #region [http 설정]
            builder.Services.AddScoped<IHttpInterceptorManager, HttpInterceptorManager>();
            builder.Services.AddScoped<AuthenticationHeaderHandler>();
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(ClientName)
                .EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    var culture = CultureInfo.DefaultThreadCurrentCulture;                    
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd($"{culture.ToString()},{culture.Name}");
                    client.BaseAddress = new Uri($"{(isHttps ? "https" : "http")}://api-service");
                })
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddSingleton<IRestClient, RestClient>();
            #endregion [http 설정]

            #region [인증 설정]
            builder.Services.AddAuthorizationCore(RegisterPermissionClaims);
            builder.Services.AddSingleton<AuthenticationStateProviderImpl>();
            builder.Services.AddSingleton<AuthenticationStateProvider, AuthenticationStateProviderImpl>();
            #endregion


            #region [계정 설정]
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<UserListViewModel>();
            #endregion

            #region [메뉴 설정]
            builder.Services.AddSingleton<MenuRoleViewModel>();
            #endregion

            #region [저장 장치 설정]
            builder.Services.AddBlazoredSessionStorageAsSingleton();
            builder.Services.AddBlazoredLocalStorageAsSingleton();
            builder.Services.AddSingleton<ISessionStorageService>(sp => 
                #if DEBUG
                    //로컬 개발은 Local storage로 함.
                new LocalSessionStorageService(sp.GetRequiredService<ILocalStorageService>())
                #else
                    //운영은 Session storage로 함.
                new SessionStorageService(sp.GetRequiredService<ISessionStorageService>())
                #endif
            );
            builder.Services.AddSingleton<IStorageService>(sp =>
                new LocalSessionStorageService(sp.GetRequiredService<ILocalStorageService>()));
            
            builder.Services.AddSingleton<ISessionStorageService, LocalSessionStorageService>();
            #endregion

            builder.Services.AddSingleton<WeatherService>();
            builder.Services.AddSingleton<WeatherViewModel>();

            builder.Services.AddSingleton<NotificationViewModel>();
            builder.Services.AddSingleton<IChartService, ChartService>();
            builder.Services.AddSingleton<INotificationService, NotificationService>();

            #region [기본 언어 설정]
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            #endregion

            builder.Services.AddSingleton<ISpinService, SpinService>();

            var build = builder.Build();
            var vm = build.Services.GetRequiredService<NotificationViewModel>();
            await vm.InitializeAsync();

            await InitializeCultureAsync(build);

            await build.RunAsync();
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
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

        private static async Task InitializeCultureAsync(WebAssemblyHost host)
        {
            var handler = host.Services.GetRequiredService<ISessionStorageService>();
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