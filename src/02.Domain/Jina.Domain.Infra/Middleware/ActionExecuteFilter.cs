using eXtensionSharp;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Infra.Middleware;

public class ActionExecuteFilter: IAsyncActionFilter
{
    private readonly ISessionContext _ctx;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="ctx"></param>
    public ActionExecuteFilter(ISessionContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {   
        //before
        _ctx.IsDecrypt = context.HttpContext.Request.Method.xEquals("get");
        
        await next();
        
        //after
    }
}

public class SessionExecuteFilter : IAsyncActionFilter
{
    private readonly ISessionContext _ctx;
    public SessionExecuteFilter(ISessionContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_ctx.TenantId.xIsEmpty()) throw new ArgumentNullException(nameof(_ctx.TenantId), "TenantId is empty");
        if (_ctx.CurrentUser.UserId.xIsEmpty()) throw new ArgumentNullException(nameof(_ctx.CurrentUser.UserId), "UserId is empty");
        
        var user = await _ctx.DbContext.xAs<AppDbContext>().Users
            .FirstOrDefaultAsync(m => m.Email == _ctx.CurrentUser.Email);
        
        if(user.xIsEmpty()) 
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (user.RefreshToken.xIsEmpty() ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (_ctx.xIsNotEmpty())
        {
            await _ctx.xAs<ISessionContextInitializer>().InitializeAsync(user);
        }

        var account = $"{_ctx.TenantId}§{_ctx.CurrentUser.UserId}";
        using (Serilog.Context.LogContext.PushProperty("Account", account))
        {
            await next();    
        }
    }
}