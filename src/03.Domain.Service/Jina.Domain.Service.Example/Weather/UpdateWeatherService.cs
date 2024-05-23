using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Example.Weather;

public sealed class UpdateWeatherService : ServiceImplBase<UpdateWeatherService, AppDbContext, UpdateWeatherRequest, IResults<int>>, IUpdateWeatherService
{
    public UpdateWeatherService(ISessionContext context, ServicePipeline pipe) : base(context, pipe)
    {
    }

    public override async Task<bool> OnExecutingAsync()
    {
        if (this.Request.Id.xIsEmptyNumber())
        {
            this.Result = await Results<int>.FailAsync("Id is empty");
            return false;
        }

        if (this.Request.UpdateDate == DateTime.MinValue)
        {
            this.Result = await Results<int>.FailAsync("UpdateDate is min date");
            return false;
        }
        
        var exist = await this.Db.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == this.Request.Id);
        if (exist.xIsEmpty())
        {
            this.Result = await Results<int>.FailAsync("not exist");
            return false;
        }

        return true;
    }

    public override async Task OnExecuteAsync()
    {
        var result = await this.Db.WeatherForecasts
                .Where(m => m.Id == this.Request.Id)
                .ExecuteUpdateAsync(m =>
                    m.SetProperty(mm => mm.Date, this.Request.UpdateDate))
            ;

        this.Result = await Results<int>.SuccessAsync(result);
    }
}