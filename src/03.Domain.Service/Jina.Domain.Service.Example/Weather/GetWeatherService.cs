using eXtensionSharp;
using Jina.Base.Service.Abstract;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Infra.Base;
using Jina.Domain.Infra.EfExtension;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
    public class GetWeatherService : EfServiceImpl<GetWeatherService, int, IResultBase<WeatherForecastDto>>, IGetWeatherService, IScopeService
    {
        public GetWeatherService(AppDbContext db, ISessionContext context) : base(db, context)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Result<WeatherForecastDto>.FailAsync("request is empty");
                return false;
            }
            return true;
        }

        public override async Task OnExecuteAsync()
        {
            var exist = await this.DbContext.WeatherForecasts.vFirstAsync(this.SessionContext, m => m.Id == this.Request, m => new WeatherForecastDto()
            {
                Id = m.Id,
                City = m.City,
                Date = m.Date,
                TemperatureC = m.TemperatureC,
                Summary = m.Summary
            });

            if (exist.xIsEmpty())
            {
                this.Result = await Result<WeatherForecastDto>.FailAsync("result is empty");
                return;
            }

            this.Result = await Result<WeatherForecastDto>.SuccessAsync(exist);
        }
    }
}