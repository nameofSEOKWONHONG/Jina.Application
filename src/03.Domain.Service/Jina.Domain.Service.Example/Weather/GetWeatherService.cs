using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadUncommitted)]
	public sealed class GetWeatherService : ServiceImplBase<GetWeatherService, AppDbContext, int, IResults<WeatherForecastResult>>
        , IGetWeatherService
    {
        public GetWeatherService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<WeatherForecastResult>.FailAsync("request is empty");
                return;
            }
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vFirstAsync(this.Ctx
                , m => m.Id == this.Request
                , m => new WeatherForecastResult()
                {
                    Id = m.Id,
                    City = m.City,
                    Date = m.Date,
                    TemperatureC = m.TemperatureC,
                    Summary = m.Summary,
                    CreatedName = m.CreatedName
                });

            this.Result = await Results<WeatherForecastResult>.SuccessAsync(exist);
        }
    }
}