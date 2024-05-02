using System.Data;
using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
    [TransactionOptions(IsolationLevel.ReadUncommitted)]
	public sealed class GetWeathersService
        : ServiceImplBase<GetWeathersService, AppDbContext, PaginatedRequest<WeatherForecastRequest>, PaginatedResult<WeatherForecastResult>>
            , IGetWeathersService
    {
        private readonly WeatherForecastResultValidator _validator;
        public GetWeathersService(ISessionContext ctx, ServicePipeline svc
        , WeatherForecastResultValidator validator) : base(ctx, svc)
        {
            _validator = validator;
        }

        public override async Task OnExecutingAsync()
        {
            if (this.Request.SearchData.From.xIsEmpty() ||
                this.Request.SearchData.To.xIsEmpty())
            {                           
                this.Result = await PaginatedResult<WeatherForecastResult>.FailAsync("Search date required");
            }
        }

        public override async Task OnExecuteAsync()
        {
            var query = this.Db.WeatherForecasts.vAsNoTrackingQueryable(this.Ctx);

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

            this.Result = await query.vToPaginatedListAsync(this.Ctx, this.Request, m => new WeatherForecastResult()
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