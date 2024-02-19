using Jina.Base.Service;
using Jina.Domain.Abstract.Net.OpenApi.Services;
using Jina.Domain.Service.Net.OpenApi.Services;
using Jina.Validate.RuleValidate;
using Jina.Validate.RuleValidate.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Jina.Test;

public class OpenApiTest
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
    public async Task openai_service_test()
    {
        var service = _serviceProvider.GetRequiredService<IOpenApiClientService>();

        var result = string.Empty;
        await ServicePipeline<string, string>.Create(service)
            .SetParameter(() => $"아래의 문장을 영어로 번역해줘.{Environment.NewLine} \"에러가 발생했습니다. 다시 시도 하시겠습니까?\"")
            .OnExecutedAsync(r => result = r);

        await TestContext.Out.WriteLineAsync(result);
    }
}