using eXtensionSharp;
using eXtensionSharp.AspNet;
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
            var isHttps = this.HttpContext.vIsHttps();
            var host = this.HttpContext.vGetBaseHost();
            var schme = this.HttpContext.vGetBaseScheme();
            var isAuth = this.HttpContext.vIsAuthenticated();
            var roles = this.HttpContext.vGetRoles();

            var controllerName = this.HttpContext.vGetControllerName();
            var methodName = this.HttpContext.vGetMethod();
            var n1 = this.HttpContext.vGetControllerFullName();
            var n2 = this.HttpContext.vGetActionName();
            var agent = this.HttpContext.vGetUserAgent();

            IResultBase<WeatherForecastDto> result = null;
            await ServicePipeline<int, IResultBase<WeatherForecastDto>>.Create(service)
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
            await ServicePipeline<PaginatedRequest<WeatherForecastDto>, PaginatedResult<WeatherForecastDto>>.Create(service)
                .AddFilter(() => request.xIsNotEmpty())
                .SetParameter(() => request)
                .OnExecutedAsync(r => result = r);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save(WeatherForecastDto request)
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