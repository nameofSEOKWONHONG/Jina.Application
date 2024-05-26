using eXtensionSharp;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Jina.Domain.Service.Infra.Middleware;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="dbContext"></param>
    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // var controllerName = context.Request.RouteValues["controller"]?.ToString();
        // var actionName = context.Request.RouteValues["action"]?.ToString();
        
        var sessionContext = context.RequestServices.GetRequiredService<ISessionContext>();
                    
        // 트랜잭션 시작
        //await using var transaction = await sessionContext.DbContext.Database.BeginTransactionAsync(transactionAttribute.IsolationLevel, cts.Token);
        try
        {
            // 다음 미들웨어 호출
            await _next(context);
            
            if(sessionContext.DbContext.Database.CurrentTransaction.xIsNotEmpty()) 
            {
                await sessionContext.DbContext.Database.CurrentTransaction!.CommitAsync();    
            }

            if (sessionContext.FSql.CurrentTransaction.xIsNotEmpty())
            {
                await sessionContext.FSql.CurrentTransaction.CommitAsync();
            }

            // if (sessionContext.DbProvider.Transaction().xIsNotEmpty())
            // {
            //     await sessionContext.DbProvider.Transaction().CommitAsync();
            // }
            
            // 모든 작업이 성공적으로 완료되면 커밋
            //await transaction.CommitAsync(cts.Token);
        }
        catch (Exception)
        {
            if(sessionContext.DbContext.Database.CurrentTransaction.xIsNotEmpty()) {
                await sessionContext.DbContext.Database.CurrentTransaction!.RollbackAsync();        
            }
            
            if (sessionContext.FSql.CurrentTransaction.xIsNotEmpty())
            {
                await sessionContext.FSql.CurrentTransaction.RollbackAsync();
            }            
            
            // if (sessionContext.DbProvider.Transaction().xIsNotEmpty())
            // {
            //     await sessionContext.DbProvider.Transaction().RollbackAsync();
            // }
            
            // 작업 중 오류 발생 시 롤백
            //await transaction.RollbackAsync(cts.Token);
            
            //global error handler에서 처리함.
            throw; 
        }
    }
}

// 확장 메서드를 사용하여 미들웨어를 구성할 수 있습니다.
public static class TransactionMiddlewareExtensions
{
    public static IApplicationBuilder UseTransactionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TransactionMiddleware>();
    }
}