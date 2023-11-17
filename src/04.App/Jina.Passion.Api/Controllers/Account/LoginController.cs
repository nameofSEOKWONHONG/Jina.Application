using Jina.Domain.Infra.Base;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Account;

public class LoginController : JControllerBase
{
    public LoginController()
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
}