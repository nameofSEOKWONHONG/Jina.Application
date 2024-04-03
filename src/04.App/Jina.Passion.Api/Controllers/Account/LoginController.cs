using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account.Token;
using Jina.Domain.Abstract.Account.User;
using Jina.Domain.Account.Request;
using Jina.Domain.Account.Token;
using Jina.Domain.Infra.Base;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Account;

public class LoginController : JControllerBase
{
    public LoginController()
    {
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(TokenRequest request
        , [FromServices] IGetTokenService service)
    {
        IResultBase<TokenResponse> result = null;

        await ServicePipeline<TokenRequest, IResultBase<TokenResponse>>.Create(service)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecutedAsync(r => result = r);

        return Ok(result);
    }

    /// <summary>
    /// jwt token 갱신
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenRequest model,
        [FromServices] IGetTokenRefreshService service)
    {
        IResultBase<TokenResponse> result = null;
        await ServicePipeline<RefreshTokenRequest, IResultBase<TokenResponse>>.Create(service)
            .AddFilter(model.xIsNotEmpty)
            .SetParameter(() => model)
            .SetValidator(new RefreshTokenRequest.Valdiator(null))
            .OnError(m =>
            {
                result = Result<TokenResponse>.Fail(m.Errors.First().ErrorMessage);
            })
            .OnExecutedAsync(m =>
            {
                result = m;
            });

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request,
        [FromServices] IRegisterUserService services)
    {
        IResultBase<bool> result = null;
        await ServicePipeline<RegisterRequest, IResultBase<bool>>.Create(services)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecutedAsync(m => result = m);

        return Ok(result);
    }
}