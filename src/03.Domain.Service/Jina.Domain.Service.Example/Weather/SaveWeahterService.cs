using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Example.Weather
{
	public sealed class SaveWeahterService : ServiceImplBase<SaveWeahterService, WeatherForecastRequest, IResults<int>>
    {
        public SaveWeahterService(ISessionContext ctx, ServicePipeline svc) : base(ctx, svc)
        {
        }

        public override async Task OnExecutingAsync()
        {
            if (this.Request.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("request is empty");
                return;
            }

            if (this.Request.City.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("city is empty");
                return;
            }

            if (this.Request.Date.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("date is empty");
                return;
            }

            if (this.Request.TemperatureC.xIsEmpty())
            {
                this.Result = await Results<int>.FailAsync("temp C. is empty");
                return;
            }
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