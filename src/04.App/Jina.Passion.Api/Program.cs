using Hangfire;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Infra.Middleware;
using Jina.Domain.Service.Net.Notification;
using Jina.Passion.Api;
using Jina.Passion.Api.Hubs;
using Microsoft.EntityFrameworkCore;
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
	// 데이터베이스 컨텍스트 서비스 가져오기
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		try
		{
			// 데이터베이스 컨텍스트 가져오기
			var context = services.GetRequiredService<AppDbContext>();

			// 데이터베이스가 없으면 데이터베이스를 생성
			//context.Database.EnsureCreated();
			//context.Database.Migrate();

			Console.WriteLine("Database created successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine("An error occurred while creating the database.");
			Console.WriteLine(ex.Message);
		}
	}

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

await app.RunAsync();