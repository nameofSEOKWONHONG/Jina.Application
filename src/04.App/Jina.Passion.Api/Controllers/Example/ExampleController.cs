using eXtensionSharp;
using eXtensionSharp.AspNet;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
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

    //     [HttpGet]
    //     public async Task<IActionResult> Get(int id,
    //         [FromServices] IGetWeatherService service)
    //     {
    //         var isHttps = this.HttpContext.xIsHttps();
    //         var host = this.HttpContext.xGetBaseHost();
    //         var schme = this.HttpContext.xGetBaseScheme();
    //         var isAuth = this.HttpContext.xIsAuthenticated();
    //         var roles = this.HttpContext.xGetRoles();
    //
    //         var controllerName = this.HttpContext.xGetControllerName();
    //         var methodName = this.HttpContext.xGetMethod();
    //         var n1 = this.HttpContext.xGetControllerFullName();
    //         var n2 = this.HttpContext.xGetActionName();
    //         var agent = this.HttpContext.xGetUserAgent();
    //
    //         IResultBase<WeatherForecastDto> result = null;
    //         await ServicePipeline<int, IResultBase<WeatherForecastDto>>.Create(service)
    //             .AddFilter(() => id.xIsNotEmpty())
    //             .SetParameter(() => id)
    //             .OnExecutedAsync(r => result = r);
    //         return Ok(result);
    //     }
    //
    //     [HttpPost]
    //     public async Task<IActionResult> Gets([FromBody] PaginatedRequest<WeatherForecastDto> request,
    //         [FromServices] IGetWeathersService service)
    //     {
    //         PaginatedResult<WeatherForecastDto> result = null;
    //         await ServicePipeline<PaginatedRequest<WeatherForecastDto>, PaginatedResult<WeatherForecastDto>>.Create(service)
    //             .AddFilter(() => request.xIsNotEmpty())
    //             .SetParameter(() => request)
    //             .OnExecutedAsync(r => result = r);
    //         return Ok(result);
    //     }
    //
    //     [HttpPost]
    //     public async Task<IActionResult> Save(WeatherForecastDto request)
    //     {
    //         return Ok();
    //     }
    //
    //     [HttpDelete]
    //     public async Task<IActionResult> Remove(int id)
    //     {
    //         return Ok();
    //     }
    }
}