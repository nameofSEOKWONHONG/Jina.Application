using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadCommitted)]
    public sealed class RemoveWeatherService : ServiceImplBase<RemoveWeatherService, AppDbContext, int, IResults>, IRemoveWeatherService
    {
        private WeatherForecast _weatherForecast;
        public RemoveWeatherService(ISessionContext context, ServicePipeline pipe) : base(context, pipe)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results.FailAsync("Id is empty.");
                return false;
            }
            
            _weatherForecast = await this.Db.WeatherForecasts.vAsNoTrackingQueryable(this.Context)
                .vFirstAsync(this.Context, m => m.Id == this.Request);

            if (this._weatherForecast.xIsEmpty())
            {
                this.Result = await Results.FailAsync("item is empty.");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            this.Db.WeatherForecasts.Remove(_weatherForecast);
            await this.Db.SaveChangesAsync();
            this.Result = await Results.SuccessAsync();
        }
    }
}
