using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadUncommitted, ENUM_DB_PROVIDER_TYPE.FreeSql)]
	public sealed class GetFreeWeatherService : ServiceImplBase<GetWeatherService, AppDbContext, int, Results<WeatherForecastResult>>
        , IGetFreeWeatherService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="pipe"></param>
        public GetFreeWeatherService(ILogger<GetWeatherService> logger, ISessionContext context, ServicePipeline pipe) : base(logger, context, pipe)
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
            var exist = await this.Context.xAs<SessionContext>()
                .FSql
                .Select<WeatherForecast>()
                .WithLock(SqlServerLock.NoLock|SqlServerLock.ReadUnCommitted)
                .Where(m => m.TenantId == this.Context.TenantId)
                .Where(m => m.Id == this.Request)
                .FirstAsync(m => new WeatherForecastResult()
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