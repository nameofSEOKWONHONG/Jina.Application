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
public class AccountController : ActionController
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
    [HttpPost]
    public async Task<IActionResult> Login(TokenRequest request
        , [FromServices] ILoginService service
        , [FromServices] TokenRequestValidator validator)
    {
        IResults<TokenResult> result = null;

        this.Pipe.Register(service)
            .When(request.xIsNotEmpty)
            .WithParameter(() => request)
            .WithValidator(() => validator)
            .ThenValidate(r => result = Results<TokenResult>.Fail(r.vErrors()))
            .Then(r => result = r);
        
        await this.Pipe.ExecuteAsync();

        this.Context.JobClient.Enqueue<EmailJob>(m => m.ExecuteAsync(new EmailRequest()));

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
        this.Pipe.Register(service)
            .When(request.xIsNotEmpty)
            .WithParameter(() => request)
            .Then(m => result = m);

        await this.Pipe.ExecuteAsync();
 
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
    [TypeFilter(typeof(ActionExecuteFilter))]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenRequest model,
        [FromServices] IRefreshTokenService service,
        [FromServices] RefreshTokenRequest.Valdiator validator)
    {
        IResults<TokenResult> result = null;

        this.Pipe.Register(service)
            .When(model.xIsNotEmpty)
            .WithParameter(() => model)
            .WithValidator(() => validator)
            .ThenValidate(r =>
            {
                result = Results<TokenResult>.Fail(r.vErrors());
            })
            .Then(m =>
            {
                result = m;
            });
        
        await this.Pipe.ExecuteAsync();
        
        return Ok(result);
    }

    /// <summary>
    /// 테넌트(ex:회사) 등록
    /// </summary>
    /// <param name="request"></param>
    /// <param name="service"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [Authorize]
    [TypeFilter(typeof(ActionExecuteFilter))]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> RegisterTenant(CreateTenantRequest request 
        , [FromServices] ICreateTenantService service
        , [FromServices] CreateTenantRequestValidator validator)
    {
        IResults result = null;
        this.Pipe.Register(service)
            .When(request.xIsNotEmpty)
            .WithParameter(() => request)
            .WithValidator(() => validator)
            .ThenValidate(m =>
            {
                result = Results.Fail(m.vErrors());
            })
            .Then(m => result = m);

        await this.Pipe.ExecuteAsync();
 
        return Ok(result);
    }
}

