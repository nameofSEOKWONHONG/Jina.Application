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
	public sealed class GetWeatherService : ServiceImplBase<GetWeatherService, AppDbContext, int, Results<WeatherForecastResult>>
        , IGetWeatherService
    {
        public GetWeatherService(ISessionContext ctx, ServicePipeline pipe) : base(ctx, pipe)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<WeatherForecastResult>.FailAsync("request is empty");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vFirstAsync(this.Context
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
    
    
    [TransactionOptions(IsolationLevel.ReadUncommitted)]
    public sealed class GetWeatherV2Service : ServiceImplBase<GetWeatherService, AppDbContext, int, IResults<WeatherForecastResult>>
        , IGetWeatherV2Service
    {
        public GetWeatherV2Service(ISessionContext ctx, ServicePipeline pipe) : base(ctx, pipe)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<WeatherForecastResult>.FailAsync("request is empty");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vFirstAsync(this.Context
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