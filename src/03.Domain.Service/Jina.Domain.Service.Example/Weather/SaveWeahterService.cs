using eXtensionSharp;
using Jina.Domain.Entity;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra.Base;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
	public class SaveWeahterService : ServiceImplBase<SaveWeahterService, WeatherForecastDto, IResultBase<int>>
    {
        public SaveWeahterService(AppDbContext db, ISessionContext context) : base(db, context)
        {
        }

        public override async Task<bool> OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Result<int>.FailAsync("request is empty");
                return false;
            }

            if (this.Request.City.xIsEmpty())
            {
                this.Result = await Result<int>.FailAsync("city is empty");
                return false;
            }

            if (this.Request.Date.xIsEmpty())
            {
                this.Result = await Result<int>.FailAsync("date is empty");
                return false;
            }

            if (this.Request.TemperatureC.xIsEmpty())
            {
                this.Result = await Result<int>.FailAsync("temp C. is empty");
                return false;
            }
            return true;
        }

        public override async Task OnExecuteAsync()
        {
            if (this.Request.Id.xIsNotEmpty())
            {
                //save
            }
            else
            {
                //update
            }
        }
    }
}