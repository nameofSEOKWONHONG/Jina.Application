using Jina.Base.Background;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate.Enums;

namespace Jina.Domain.Service.Net.ExchangeRate
{
    public class SaveExchangeRateBackgroundService : BackgroundServiceBase<SaveExchangeRateBackgroundService, ExchangeRequest>
    {
        private readonly ISaveExchangeRateService _service;
        public SaveExchangeRateBackgroundService(ISaveExchangeRateService service) : base(0, 1000)
        {
            _service = service;
        }

        protected override async Task<IEnumerable<ExchangeRequest>> OnProducerAsync(CancellationToken stoppingToken)
        {
            Logger.Information("start {name} service", nameof(SaveExchangeRateBackgroundService));

            ENUM_EXCHANGE_RESULT_TYPE result = null;
            await ServiceInvoker<ExchangeRequest, ENUM_EXCHANGE_RESULT_TYPE>.Invoke(_service)
                .SetParameter(() => new ExchangeRequest()
                {
                    AuthKey = "",
                    SearchDate = DateTime.Now,
                    SearchType = ENUM_EXCHANGE_DATA_TYPE.AP01
                })
                .OnExecutedAsync(r => result = r);

            Logger.Information("end {name} service", nameof(SaveExchangeRateBackgroundService));

            return null;
        }

        protected override Task OnConsumerAsync(ExchangeRequest request, CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
