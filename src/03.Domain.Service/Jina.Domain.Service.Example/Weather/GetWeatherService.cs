using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
	public class GetWeatherService : ServiceImplBase<GetWeatherService, AppDbContext, int, IResultBase<WeatherForecastDto>>, IGetWeatherService, IScopeService
    {
        public GetWeatherService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await ResultBase<WeatherForecastDto>.FailAsync("request is empty");
                return false;
            }
            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vFirstAsync(this.SessionContext, m => m.Id == this.Request, m => new WeatherForecastDto()
            {
                Id = m.Id,
                City = m.City,
                Date = m.Date,
                TemperatureC = m.TemperatureC,
                Summary = m.Summary
            });

            if (exist.xIsEmpty())
            {
                this.Result = await ResultBase<WeatherForecastDto>.FailAsync("result is empty");
                return;
            }

            this.Result = await ResultBase<WeatherForecastDto>.SuccessAsync(exist);
        }
    }
}