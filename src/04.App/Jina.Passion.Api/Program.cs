using Hangfire;
using Jina.Domain.Service.Infra.Middleware;
using Jina.Domain.Service.Net.Notification;
using Jina.Passion.Api;
using Jina.Passion.Api.Controllers.Example;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
	loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = WebApplicationBuilderInitializer
    .Initialize(builder)
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
	.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseCors("AllowedCorsOrigins"); 
app.MapControllers();
app.UseHangfireDashboard("/hangfire");
app.UseRouting();

#region [cache header]

//app.UseHttpCacheHeaders();

#endregion


app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();

//인증
app.UseAuthentication();
//권한
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseResponseCompression();
app.MapHub<MessageHub>("/messageHub");
app.UseTransactionMiddleware();

//init ticket data
await TicketImpl.Instance.SetZone("A");
//ticket sold out event.
TicketImpl.Instance.OnSoldOut = async () =>
{
	//use hangfire, or direct sending.
	
	//logic...
	//check ticket sold out states. (database)
	//var status = await Db.TicketStatus.FirstOrDefaultAsync(m => m.Id == TicketImpl.Instance.Id && m.ZoneId == TicketImpl.Insatnce.ZoneId);
	//if(status.IsComplete == "Y") return;
	
	//insert or update ticket states. (database)
	//var ticketList = TicketImpl.Instance.Complete.ToList();
	//foreach(var item in ticketList) {
	//await Db.Tickets.Where(m => m.Id == TicketImpl.Instance.Id).ExecuteUpdateAsync(m => m.SetProperty(mm => mm.Status, "Y");
	//}
	
	//send admin notification.
	//await emailService.Send("admin", "admin@test.com", "ticket sold out.");
	
	//last, logging.
	await Task.Delay(1000);
	Log.Logger.Information("ticket sold out");
};

await app.RunAsync();