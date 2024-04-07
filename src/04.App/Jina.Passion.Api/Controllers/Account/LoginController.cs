using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Account.Token;
using Jina.Domain.Service.Infra;
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
        , [FromServices] ILoginService service)
    {
        IResultBase<TokenResult> result = null;

        await ServicePipeline<TokenRequest, IResultBase<TokenResult>>.Create(service)
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
    //[AllowAnonymous]
    //[HttpPost]
    //public async Task<IActionResult> Refresh(RefreshTokenRequest model,
    //    [FromServices] IGetTokenRefreshService service)
    //{
    //    IResultBase<TokenResult> result = null;
    //    await ServicePipeline<RefreshTokenRequest, IResultBase<TokenResult>>.Create(service)
    //        .AddFilter(model.xIsNotEmpty)
    //        .SetParameter(() => model)
    //        .SetValidator(new RefreshTokenRequest.Valdiator(null))
    //        .OnError(m =>
    //        {
    //            result = Result<TokenResult>.Fail(m.Errors.First().ErrorMessage);
    //        })
    //        .OnExecutedAsync(m =>
    //        {
    //            result = m;
    //        });

    //    return Ok(result);
    //}

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request,
        [FromServices] IRegisterUserService service)
    {
        IResultBase<bool> result = null;
        await ServicePipeline<RegisterRequest, IResultBase<bool>>.Create(service)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecutedAsync(m => result = m);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AdminInit(CreateTenantRequest request, 
        [FromServices] ICreateTenantService service)
    {
		IResultBase<bool> result = null;
		await ServicePipeline<CreateTenantRequest, IResultBase<bool>>.Create(service)
			.AddFilter(() => request.xIsNotEmpty())
			.SetParameter(() => request)
			.OnExecutedAsync(m => result = m);

		return Ok(result);
	}
}