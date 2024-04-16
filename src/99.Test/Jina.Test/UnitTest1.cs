using Jina.Base.Service;
using Jina.Database;
using Jina.Database.Abstract;
using Jina.Domain.Abstract.Net.OpenApi.Services;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Net.OpenApi.Services;
using Jina.Lang;
using Jina.Lang.Abstract;
using Jina.Session.Abstract;
using Jina.Validate.RuleValidate;
using Jina.Validate.RuleValidate.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jina.Test;

public class OpenApiTest
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHttpContextAccessor();
        services.AddScoped<ISessionContext, SessionContext>();
        services.AddScoped<ISessionCurrentUser, SessionCurrentUser>(provider =>
        {
            //TODO : custom IHttpContextAccessor
            return new SessionCurrentUser(null);
        });
        services.AddScoped<ISessionDateTime, SessionDateTime>();
        services.AddTransient<ServicePipeline>();
        services.AddTransient<ILocalizer, Localizer>();
        services.AddScoped<IOpenApiClientService, OpenApiClientService>();
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "jina"));
        services.AddTransient<IDbProviderBase, MsSqlProvider>();
        services.AddHttpClient();
        _serviceProvider = services.BuildServiceProvider();
        
        
    }

    [Test]
    public async Task openai_service_test()
    {
        var pip = _serviceProvider.GetRequiredService<ServicePipeline>();
        var service = _serviceProvider.GetRequiredService<IOpenApiClientService>();

        var result = string.Empty;
        pip.Register(service)
            .SetParameter(() => $"아래의 문장을 영어로 번역해줘.{Environment.NewLine} \"에러가 발생했습니다. 다시 시도 하시겠습니까?\"")
            .OnExecuted(r => result = r);
        await pip.ExecuteAsync();

        Assert.That(result, Is.Not.Null);
        
        await TestContext.Out.WriteLineAsync(result);
        
    }
}