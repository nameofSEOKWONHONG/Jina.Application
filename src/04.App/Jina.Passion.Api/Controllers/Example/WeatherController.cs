using eXtensionSharp;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Entity.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jina.Passion.Api.Controllers.Example;

public class WeatherController : SessionRestController
{
    public WeatherController()
    {
    }

     //ETag는 동일 url 패턴 속성에서만 동작한다.
    [HttpGet("/api/[controller]/{id}")]
    public async Task<IActionResult> Get(int id,
        [FromServices] IGetWeatherService service)
    {
        IResults<WeatherForecastResult> result = null;
        
        this.Pipe.Register(service)
            .AddFilter(() => id > 0)
            .SetParameter(() => id)
            .UseCache()
            .OnExecuted(r => result = r);
        
        await this.Pipe.ExecuteAsync();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList(PaginatedRequest<WeatherForecastRequest> request
        , [FromServices] IGetWeathersService service)
    {
        PaginatedResult<WeatherForecastResult> result = null;
        this.Pipe.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(r => result = r);
        await this.Pipe.ExecuteAsync();
        return Ok(result);
    }

    [HttpPost("init")]
    public async Task<IActionResult> InitData()
    {
        var exist = await this.Context.DbContext.xAs<AppDbContext>().WeatherForecasts.FirstOrDefaultAsync();
        if (exist.xIsNotEmpty())
        {
            await this.Context.DbContext.Database.ExecuteSqlRawAsync($"truncate table {nameof(WeatherForecast)}s");
        }
        
        List<WeatherForecast> items = new();
        List<string> cities = new List<string>()
        {
            "Delaware", "Pennsylvania", "New jersey", "Georgia", "Connecticut", "Massachusetts"
        };
        Enumerable.Range(1, 500).xForEach(i =>
        {
            items.Add(new WeatherForecast()
            {
                City = cities[Random.Shared.Next(0, cities.Count-1)],
                Date = DateTime.Now.AddDays(Random.Shared.Next(-30, 30)),
                TemperatureC = Random.Shared.Next(10, 50),
                Summary = "test",
            });
        });
        await this.Context.DbContext.xAs<AppDbContext>().WeatherForecasts.AddRangeAsync(items);
        await this.Context.DbContext.xAs<AppDbContext>().SaveChangesAsync();
    
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Save(WeatherForecastResult request
        , [FromServices] ISaveWeatherService service)
    {
        IResults<int> result = null;
        
        this.Pipe.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(r => result = r);

        await this.Pipe.ExecuteAsync();

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateDate(UpdateWeatherRequest request
        , [FromServices] IUpdateWeatherService service)
    {
        IResults<int> result = null;
        
        this.Pipe.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(r => result = r);

        await this.Pipe.ExecuteAsync();

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(int request
        , [FromServices] IRemoveWeatherService service)
    {
        IResults result = null;
        
        this.Pipe.Register(service)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecuted(r => result = r);

        await this.Pipe.ExecuteAsync();

        return Ok(result);
    }
}