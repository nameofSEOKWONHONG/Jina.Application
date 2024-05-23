using Jina.Domain.Service.Example.FreeSqlExample;
using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example;

public class FreeSqlController : ActionController
{
    private readonly IWeatherService _weatherService;
    public FreeSqlController(IWeatherService service)
    {
        _weatherService = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string tenantId, int id)
    {
        var result = await _weatherService.Get(tenantId, id);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(WeatherUpdateRequest request)
    {
        var result = await _weatherService.Update(request);
        return Ok(result);
    }
}