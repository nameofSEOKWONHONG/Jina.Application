using eXtensionSharp;
using Jina.Base.Attributes;
using Jina.Domain.Abstract.Account;
using Jina.Domain.Account.Token;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IsolationLevel = System.Data.IsolationLevel;

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
    [TransactionOptions(IsolationLevel.Snapshot)]
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

    /// <summary>
    /// token 갱신
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [TransactionOptions(IsolationLevel.Snapshot)]
    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenRequest model,
        [FromServices] IRefreshTokenService service)
    {
        IResultBase<TokenResult> result = null;

        this.Pip.Register(service)
            .AddFilter(model.xIsNotEmpty)
            .SetParameter(() => model)
            .SetValidator(new RefreshTokenRequest.Valdiator(null))
            .OnError(m =>
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

 //    /// <summary>
 //    /// 사용자 등록
 //    /// </summary>
 //    /// <param name="request"></param>
 //    /// <param name="service"></param>
 //    /// <returns></returns>
 //    [AllowAnonymous]
 //    [HttpPost]
 //    public async Task<IActionResult> Register(RegisterRequest request,
 //        [FromServices] IRegisterUserService service)
 //    {
 //        IResultBase<bool> result = null;
 //        await ServicePipeline<RegisterRequest, IResultBase<bool>>.Create(service)
 //            .AddFilter(request.xIsNotEmpty)
 //            .SetParameter(() => request)
 //            .OnExecutedAsync(m => result = m);
 //
 //        return Ok(result);
 //    }
 //
 //    /// <summary>
 //    /// 관리자 등록
 //    /// </summary>
 //    /// <param name="request"></param>
 //    /// <param name="service"></param>
 //    /// <returns></returns>
 //    [ApiExplorerSettings(IgnoreApi = true)]
 //    [AllowAnonymous]
 //    [HttpPost]
 //    public async Task<IActionResult> AdminInit(CreateTenantRequest request, 
 //        [FromServices] ICreateTenantService service)
 //    {
	// 	IResultBase<bool> result = null;
	// 	await ServicePipeline<CreateTenantRequest, IResultBase<bool>>.Create(service)
	// 		.AddFilter(request.xIsNotEmpty)
	// 		.SetParameter(() => request)
	// 		.OnExecutedAsync(m => result = m);
 //
	// 	return Ok(result);
	// }
}