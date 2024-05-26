using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadCommitted, ENUM_DB_PROVIDER_TYPE.FreeSql)]
    public sealed class RemoveFreeWeatherService : ServiceImplBase<RemoveWeatherService, AppDbContext, int, IResults>
        , IRemoveFreeWeatherService
    {
        public RemoveFreeWeatherService(ILogger<RemoveWeatherService> logger, ISessionContext ctx) : base(logger, ctx)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results.FailAsync("Id is empty.");
                return false;
            }
            
            var exist = await this.Context.FSql
                .Select<WeatherForecast>()
                .Where(m => m.TenantId == this.Context.TenantId)
                .Where(m => m.Id == this.Request)
                .FirstAsync();

            if (exist.xIsEmpty())
            {
                this.Result = await Results.FailAsync("item is empty.");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var result = await this.Context.FSql
                .Delete<WeatherForecast>()
                .Where(m => m.TenantId == this.Context.TenantId)
                .Where(m => m.Id == this.Request)
                .ExecuteAffrowsAsync();

            this.Result = result > 0 ? await Results.SuccessAsync() : await Results.FailAsync();

            throw new Exception("test");
        }
    }
}
