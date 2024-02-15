using Hangfire;
using Jina.Domain.Entity.Net.ExchangeRate;
using Jina.Passion.Api;
using Jina.Passion.Api.Hubs;

var app = WebApplicationBuilderInitializer
    .Initialize(WebApplication.CreateBuilder(args))
    .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles()
   .UseBlazorFrameworkFiles()
   .UseHttpsRedirection()
   .UseAuthorization();
app.UseCors("AllowedCorsOrigins");
app.MapControllers();
app.UseHangfireDashboard("/hangfire");
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseResponseCompression();
app.MapHub<ProtocalHub>("/messageHub");

await app.RunAsync();