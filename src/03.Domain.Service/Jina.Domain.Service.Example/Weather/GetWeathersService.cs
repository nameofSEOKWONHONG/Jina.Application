using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using Jina.Validate;

namespace Jina.Domain.Service.Example.Weather
{
	public sealed class GetWeathersService
        : ServiceImplBase<GetWeathersService, AppDbContext, PaginatedRequest<WeatherForecastRequest>, PaginatedResult<WeatherForecastRequest>>
            , IGetWeathersService
    {
        private readonly WeatherForecastRequestValidator _validator;
        public GetWeathersService(ISessionContext ctx, ServicePipeline svc
        , WeatherForecastRequestValidator validator) : base(ctx, svc)
        {
            _validator = validator;
        }

        public override async Task OnExecutingAsync()
        {
            var valid = await _validator.ValidateAsync(this.Request.SearchData);
            if (valid.IsValid.xIsFalse())
            {
                this.Result = await PaginatedResult<WeatherForecastRequest>.FailAsync(valid.vToErrors());
            }
            if (this.Request.xIsEmpty())
            {
                this.Result = await PaginatedResult<WeatherForecastRequest>.FailAsync("request is empty");
                return;
            }

            if (this.Result.Data.xIsEmpty())
            {
                this.Result = await PaginatedResult<WeatherForecastRequest>.FailAsync("request data is empty");
                return;
            }
        }

        public override async Task OnExecuteAsync()
        {
            var query = this.Db.WeatherForecasts.AsQueryable();

            if (this.Request.SearchData.City.xIsNotEmpty())
            {
                query = query.Where(m => m.City.Contains(this.Request.SearchData.City));
            }
            if (this.Request.From.xIsNotEmpty() && this.Request.To.xIsNotEmpty())
            {
                query = query.Where(m => m.Date >= this.Request.From.Value &&
                                         m.Date < this.Request.To.Value.xToToDate(false));
            }

            this.Result = await query.vToPaginatedListAsync(this.Ctx, this.Request, m => new WeatherForecastRequest()
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