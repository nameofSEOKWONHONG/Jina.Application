using System.Text;
using eXtensionSharp;
using Hangfire;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Account.Token;
using Jina.Domain.Net;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Infra.Middleware;
using Jina.Domain.Service.Net;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Validate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Results = Jina.Domain.Shared.Results;

namespace Jina.Passion.Api.Controllers.Account;

/// <summary>
/// 계정 컨트롤러
/// </summary>
public class AccountController : JControllerBase
{
    public AccountController()
    {
        
    }

    /// <summary>
    /// 로그인
    /// </summary>
    /// <param name="request"></param>
    /// <param name="service"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Login(TokenRequest request
        , [FromServices] ILoginService service
        , [FromServices] TokenRequestValidator validator)
    {
        IResults<TokenResult> result = null;

        this.Spl.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .SetValidator(() => validator)
            .OnValidated(vResult => result = Results<TokenResult>.Fail(vResult.Errors.xJoin()))
            .OnExecuted(r => result = r);
        
        await this.Spl.ExecuteAsync();

        this.Ctx.JobClient.Enqueue<EmailJob>(m => m.ExecuteAsync(new EmailRequest()));

        return Ok(result);
    }
    
    [Authorize]
    [TypeFilter(typeof(ActionExecuteFilter))]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Logout(LogoutRequest request,
        [FromServices] ILogoutService service)
    {
        IResults<bool> result = null;
        this.Spl.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(m => result = m);

        await this.Spl.ExecuteAsync();
 
        return Ok(result);
    }

    /// <summary>
    /// token 갱신
    /// </summary>
    /// <param name="model"></param>
    /// <param name="service"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [Authorize]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenRequest model,
        [FromServices] IRefreshTokenService service,
        [FromServices] RefreshTokenRequest.Valdiator validator)
    {
        IResults<TokenResult> result = null;

        this.Spl.Register(service)
            .AddFilter(model.xIsNotEmpty)
            .SetParameter(() => model)
            .SetValidator(() => validator)
            .OnValidated(m =>
            {
                result = Results<TokenResult>.Fail(m.vToKeyValueErrors());
            })
            .OnExecuted(m =>
            {
                result = m;
            });
        
        await this.Spl.ExecuteAsync();
        
        return Ok(result);
    }

    /// <summary>
    /// 테넌트(ex:회사) 등록
    /// </summary>
    /// <param name="request"></param>
    /// <param name="service"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> RegisterTenant(CreateTenantRequest request 
        , [FromServices] ICreateTenantService service
        , [FromServices] CreateTenantRequestValidator validator)
    {
        IResultBase result = null;
        this.Spl.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .SetValidator(() => validator)
            .OnValidated(m =>
            {
                result = Results.Fail(m.vToKeyValueErrors());
            })
            .OnExecuted(m => result = m);

        await this.Spl.ExecuteAsync();
 
        return Ok(result);
    }
}

