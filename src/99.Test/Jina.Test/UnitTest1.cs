using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.OpenApi.Services;
using Jina.Domain.Service.Net.OpenApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jina.Test;

public class Tests
{
    private IServiceProvider _serviceProvider;


    [SetUp]
    public void Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IOpenApiClientService, OpenApiClientService>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public async Task Test1()
    {
        var service = _serviceProvider.GetRequiredService<IOpenApiClientService>();
        
        var result = string.Empty;
        await JServiceInvoker<string, string>.Invoke(service)
            .SetParameter(() => $"아래의 문장을 영어로 번역해줘.{Environment.NewLine} \"에러가 발생했습니다. 다시 시도 하시겠습니까?\"")
            .ExecutedAsync(r => result = r);

        await TestContext.Out.WriteLineAsync(result);
    }
}