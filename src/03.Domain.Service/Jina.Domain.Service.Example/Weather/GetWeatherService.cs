using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
	public sealed class GetWeatherService : ServiceImplBase<GetWeatherService, AppDbContext, int, IResults<WeatherForecastRequest>>, IGetWeatherService, IScopeService
    {
        public GetWeatherService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<WeatherForecastRequest>.FailAsync("request is empty");
                return;
            }
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vFirstAsync(this.Ctx, m => m.Id == this.Request, m => new WeatherForecastRequest()
            {
                Id = m.Id,
                City = m.City,
                Date = m.Date,
                TemperatureC = m.TemperatureC,
                Summary = m.Summary
            });

            if (exist.xIsEmpty())
            {
                this.Result = await Results<WeatherForecastRequest>.FailAsync("result is empty");
                return;
            }

            this.Result = await Results<WeatherForecastRequest>.SuccessAsync(exist);
        }
    }
}