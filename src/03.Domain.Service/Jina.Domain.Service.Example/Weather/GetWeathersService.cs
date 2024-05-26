using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadUncommitted)]
	public sealed class GetWeathersService
        : ServiceImplBase<GetWeathersService, AppDbContext, PaginatedRequest<WeatherForecastRequest>, PaginatedResult<WeatherForecastResult>>
            , IGetWeathersService
    {
        public GetWeathersService(ILogger<GetWeathersService> logger, ISessionContext ctx) : base(logger, ctx)
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
            var query = this.Db.WeatherForecasts.vAsNoTrackingQueryable(this.Context);

            if (this.Request.SearchData.City.xIsNotEmpty())
            {
                query = query.Where(m => m.City.Contains(this.Request.SearchData.City));
            }

            if (this.Request.SearchData.From.xIsNotEmpty() &&
                this.Request.SearchData.To.xIsNotEmpty())
            {
                query = query.Where(m => m.Date >= this.Request.SearchData.From.Value &&
                                         m.Date < this.Request.SearchData.To.Value.xToToDate(false));                
            }

            this.Result = await query.vToPaginatedListAsync(this.Context, this.Request, m => new WeatherForecastResult()
            {
                Id = m.Id,
                City = m.City,
                Date = m.Date,
                TemperatureC = m.TemperatureC,
                Summary = m.Summary,
            });
        }
    }
}