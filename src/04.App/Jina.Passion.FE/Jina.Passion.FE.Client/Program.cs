using AntDesign.ProLayout;
using Jina.Passion.FE.Client.Pages.Weather.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAntDesign();
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    return client;
});
builder.Services.Configure<ProSettings>(config =>
{
    config = builder.Configuration.GetSection("ProSettings").Get<ProSettings>();
});
builder.Services.AddScoped<WeatherService>();

await builder.Build().RunAsync();