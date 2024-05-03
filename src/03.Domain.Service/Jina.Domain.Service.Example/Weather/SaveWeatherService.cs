using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Example.Weather
{   
    [TransactionOptions(IsolationLevel.ReadCommitted)]
	public sealed class SaveWeatherService : ServiceImplBase<SaveWeatherService, AppDbContext, WeatherForecastResult, IResults<int>>, ISaveWeatherService
    {
        public SaveWeatherService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("request is empty");
                return;
            }

            if (this.Request.City.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("city is empty");
                return;
            }

            if (this.Request.Date.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("date is empty");
                return;
            }

            if (this.Request.TemperatureC.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("temp C. is empty");
                return;
            }
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts
                .FirstOrDefaultAsync(m => m.Id == this.Request.Id);
            
            if (exist.xIsEmpty())
            {
                var converted= this.Request.Adapt<WeatherForecast>();
                this.Db.WeatherForecasts.Add(converted);
                await this.Db.SaveChangesAsync();
            }
            else
            {
                await this.Db.WeatherForecasts
                    .ExecuteUpdateAsync(m =>
                        m.SetProperty(mm => mm.Summary, this.Request.Summary)
                            .SetProperty(mm => mm.TemperatureC, this.Request.TemperatureC)
                            .SetProperty(mm => mm.City, this.Request.City)
                            .SetProperty(mm => mm.Date, this.Request.Date));
                await this.Db.SaveChangesAsync();
            }
        }
    }
}