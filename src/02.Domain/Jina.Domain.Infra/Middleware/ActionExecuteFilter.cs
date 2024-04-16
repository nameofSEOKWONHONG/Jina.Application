using EntityFramework.DynamicLinq;
using eXtensionSharp;
using Jina.Domain.Entity;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
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
        _ctx.IsDecrypt = string.Equals(context.HttpContext.Request.Method, "get", StringComparison.InvariantCultureIgnoreCase);
        var user = await _ctx.DbContext.xAs<AppDbContext>().Users
            .FirstAsync(m => m.TenantId == _ctx.TenantId && m.Email == _ctx.CurrentUser.Email);
        if (user.RefreshToken.xIsEmpty() && user.RefreshTokenExpiryTime == DateTime.MinValue)
        {
            throw new Exception("Retry authorize");
        }
        
        await next();
        
        //after
    }
}