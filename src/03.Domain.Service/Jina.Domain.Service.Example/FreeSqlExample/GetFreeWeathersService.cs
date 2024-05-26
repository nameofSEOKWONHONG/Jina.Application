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
	public sealed class GetFreeWeathersService
        : ServiceImplBase<GetWeathersService, AppDbContext, PaginatedRequest<WeatherForecastRequest>, PaginatedResult<WeatherForecastResult>>
            , IGetFreeWeathersService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="pipe"></param>
        public GetFreeWeathersService(ILogger<GetWeathersService> logger, ISessionContext context, ServicePipeline pipe) : base(logger, context, pipe)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.SearchData.From.xIsEmpty() ||
                this.Request.SearchData.To.xIsEmpty())
            {                           
                this.Result = await PaginatedResult<WeatherForecastResult>.FailAsync("Search date required");
                return false;
            }


            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var query = this.Context.xAs<SessionContext>()
                .FSql
                .Select<WeatherForecast>()
                .WithLock(SqlServerLock.NoLock|SqlServerLock.ReadUnCommitted)
                .WhereIf(this.Request.SearchData.City.xIsNotEmpty(), m => m.City.Contains(this.Request.SearchData.City))
                .WhereIf(this.Request.SearchData.From.xIsNotEmpty() &&
                         this.Request.SearchData.To.xIsNotEmpty(), m => m.Date >= this.Request.SearchData.From.Value &&
                                                                        m.Date < this.Request.SearchData.To.Value
                                                                            .xToToDate(false));
            var total = await query.CountAsync();
            var result = await query
                .Page(this.Request.PageNo, this.Request.PageSize)
                .ToListAsync(m => new WeatherForecastResult()
                {
                    Id = m.Id,
                    City = m.City,
                    Date = m.Date,
                    TemperatureC = m.TemperatureC,
                    Summary = m.Summary,
                });

            this.Result = await PaginatedResult<WeatherForecastResult>.SuccessAsync(result, (int)total,
                this.Request.PageNo, this.Request.PageSize);
        }
    }
}