﻿using Jina.Base.Background;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate.Enums;
using Jina.Session.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Net.ExchangeRate
{
    public class SaveExchangeRateBackgroundService : BackgroundServiceBase<SaveExchangeRateBackgroundService, ExchangeRequest>
    {
        public SaveExchangeRateBackgroundService(ILogger<SaveExchangeRateBackgroundService> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory, 0, (1000 * 60))
        {
            
        }

        protected override async Task<IEnumerable<ExchangeRequest>> OnProducerAsync(CancellationToken stoppingToken)
        {
            this.Logger.LogInformation("start {name} service", nameof(SaveExchangeRateBackgroundService));

            bool result = false;
            var scope = this.ServiceScopeFactory.CreateAsyncScope();

            //TODO : USD -> KRW 이외에 기타 환율이 필요하다면 구현되어야 하고 다수의 서비스를 호출하도록 변경되어야 함.
            //즉, GetKeyedServices<>(""); 로 변경되어야 함.
            
            var service = scope.ServiceProvider.GetService<ISaveExchangeRateService>();
            var context = scope.ServiceProvider.GetService<ISessionContext>();
            
            using var pipeline = new ServicePipeline(context);
            pipeline.Register(service)
                .Then(r => result = r);
            await pipeline.ExecuteAsync();

            this.Logger.LogInformation("end {name} service", nameof(SaveExchangeRateBackgroundService));

            return null;
        }

        protected override Task OnConsumerAsync(ExchangeRequest request, CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
