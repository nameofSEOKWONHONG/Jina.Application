using eXtensionSharp;
using eXtensionSharp.AspNet;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example;

public class ExampleController : SessionController
{
    public ExampleController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id,
        [FromServices] IGetWeatherService service)
    {
        IResults<WeatherForecastDto> result = null;
        this.Spl.Register(service)
            .AddFilter(() => id.xIsNotEmpty())
            .SetParameter(() => id)
            .OnExecuted(r => result = r);
        await this.Spl.ExecuteAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Gets([FromBody] PaginatedRequest<WeatherForecastDto> request,
        [FromServices] IGetWeathersService service)
    {
        PaginatedResult<WeatherForecastDto> result = null;
        this.Spl.Register(service)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecuted(r => result = r);
        await this.Spl.ExecuteAsync();
        return Ok(result);
    }
}