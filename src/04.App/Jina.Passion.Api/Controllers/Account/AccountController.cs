using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Request;
using Jina.Domain.Account.Token;
using Jina.Domain.Service.Infra;
using Jina.Domain.Service.Infra.Middleware;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        , [FromServices] ILoginService service)
    {
        IResultBase<TokenResult> result = null;

        this.Pip.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(r => result = r);
        
        await this.Pip.ExecuteAsync();

        return Ok(result);
    }
    
    [Authorize]
    [TypeFilter(typeof(ActionExecuteFilter))]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Logout(LogoutRequest request,
        [FromServices] ILogoutService service)
    {
        IResultBase<bool> result = null;
        this.Pip.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(m => result = m);

        await this.Pip.ExecuteAsync();
 
        return Ok(result);
    }

    /// <summary>
    /// token 갱신
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenRequest model,
        [FromServices] IRefreshTokenService service)
    {
        IResultBase<TokenResult> result = null;

        this.Pip.Register(service)
            .AddFilter(model.xIsNotEmpty)
            .SetParameter(() => model)
            .SetValidator(new RefreshTokenRequest.Valdiator(null))
            .OnValidated(m =>
            {
                result = ResultBase<TokenResult>.Fail(m.Errors.First().ErrorMessage);
            })
            .OnExecuted(m =>
            {
                result = m;
            });
        
        await this.Pip.ExecuteAsync();
        
        return Ok(result);
    }
    
    /// <summary>
    /// 테넌트(ex:회사) 등록
    /// </summary>
    /// <param name="request"></param>
    /// <param name="service"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [TransactionOptions]
    [HttpPost]
    public async Task<IActionResult> RegisterTenant(CreateTenantRequest request, 
        [FromServices] ICreateTenantService service)
    {
        IResultBase<bool> result = null;
        this.Pip.Register(service)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .SetValidator(new CreateTenantRequest.Validator())
            .OnValidated(m => result = ResultBase<bool>.Fail(m.Errors.xJoin()))
            .OnExecuted(m => result = m);

        await this.Pip.ExecuteAsync();
 
        return Ok(result);
    }


}