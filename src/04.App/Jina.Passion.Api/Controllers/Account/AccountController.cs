using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Account;

public class AccountController : JControllerBase
{
    public AccountController()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Gets()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Save()
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove()
    {
        return Ok();
    }
}