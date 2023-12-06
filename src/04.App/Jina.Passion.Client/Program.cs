using AntDesign.ProLayout;
using Jina.Passion.Client.Pages.Account.Services;
using Jina.Passion.Client.Pages.Account.ViewModels;
using Jina.Passion.Client.Pages.Weather.Services;
using Jina.Passion.Client.Pages.Weather.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Jina.Passion.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
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

            builder.Services.AddScoped<WeatherService>();
            builder.Services.AddScoped<WeatherViewModel>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserListViewModel>();

            await builder.Build().RunAsync();
        }
    }
}