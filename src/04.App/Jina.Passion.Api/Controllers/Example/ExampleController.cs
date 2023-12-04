using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Infra.Base;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example
{
    public class ExampleController : JControllerBase
    {
        public ExampleController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id,
            [FromServices] IGetWeatherService service)
        {
            IResultBase<WeatherForecastDto> result = null;
            await ServiceInvoker<int, IResultBase<WeatherForecastDto>>.Invoke(service)
                .AddFilter(() => id.xIsNotEmpty())
                .SetParameter(() => id)
                .OnExecutedAsync(r => result = r);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Gets([FromBody] PaginatedRequest<WeatherForecastDto> request,
            [FromServices] IGetWeathersService service)
        {
            PaginatedResult<WeatherForecastDto> result = null;
            await ServiceInvoker<PaginatedRequest<WeatherForecastDto>, PaginatedResult<WeatherForecastDto>>.Invoke(service)
                .AddFilter(() => request.xIsNotEmpty())
                .SetParameter(() => request)
                .OnExecutedAsync(r => result = r);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] WeatherForecastDto request)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            return Ok();
        }
    }
}