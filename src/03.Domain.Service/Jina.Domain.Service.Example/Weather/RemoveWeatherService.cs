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
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadCommitted)]
    public sealed class RemoveWeatherService : ServiceImplBase<RemoveWeatherService, AppDbContext, int, IResults>, IRemoveWeatherService
    {
        public RemoveWeatherService(ILogger<RemoveWeatherService> logger, ISessionContext ctx) : base(logger, ctx)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results.FailAsync("Id is empty.");
                return false;
            }
            
            var exist = await this.Db.WeatherForecasts.vAsNoTrackingQueryable(this.Context)
                .vFirstAsync(this.Context, m => m.Id == this.Request);

            if (exist.xIsEmpty())
            {
                this.Result = await Results.FailAsync("item is empty.");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.Db.WeatherForecasts.vAsNoTrackingQueryable(this.Context)
                .vFirstAsync(this.Context, m => m.Id == this.Request);
            this.Db.WeatherForecasts.Remove(exist);
            await this.Db.SaveChangesAsync();
            this.Result = await Results.SuccessAsync();
        }
    }
}
