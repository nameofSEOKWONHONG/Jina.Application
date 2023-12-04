using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.EfExtension;
using Jina.Domain.SharedKernel;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
    public class GetWeathersService
        : EfServiceImpl<GetWeathersService, PaginatedRequest<WeatherForecastDto>, PaginatedResult<WeatherForecastDto>>
            , IGetWeathersService
            , IScopeService
    {
        public GetWeathersService(AppDbContext db, ISessionContext context) : base(db, context)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await PaginatedResult<WeatherForecastDto>.FailAsync("request is empty");
                return false;
            }

            if (this.Result.Data.xIsEmpty())
            {
                this.Result = await PaginatedResult<WeatherForecastDto>.FailAsync("request data is empty");
                return false;
            }

            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var query = this.DbContext.WeatherForecasts.AsQueryable();

            if (this.Request.SearchOption.City.xIsNotEmpty())
            {
                query = query.Where(m => m.City.Contains(this.Request.SearchOption.City));
            }
            if (this.Request.From.xIsNotEmpty() && this.Request.To.xIsNotEmpty())
            {
                query = query.Where(m => m.Date >= this.Request.From.Value &&
                                         m.Date < this.Request.To.Value.xToToDate(false));
            }

            this.Result = await query.vToPaginatedListAsync(this.SessionContext, this.Request, m => new WeatherForecastDto()
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