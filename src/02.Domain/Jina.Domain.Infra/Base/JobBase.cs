using Jina.Base.Service;
using Microsoft.AspNetCore.SignalR;

namespace Jina.Domain.Service.Infra;

/// <summary>
/// Not support session context, If you need information about the session, pass it as a parameter.
/// </summary>
public abstract class JobBase<TRequest>
{
    protected ServicePipeline Spl;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="spl"></param>
    protected JobBase(ServicePipeline spl)
    {
        this.Spl = spl;
    }

    public abstract Task ExecuteAsync(TRequest request);
}

public abstract class JobBase<THub, TRequest> : JobBase<TRequest>
    where THub : Hub
{
    protected IHubContext<THub> HubContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spl"></param>
    protected JobBase(ServicePipeline spl, IHubContext<THub> hubContext) : base(spl)
    {
        HubContext = hubContext;
    }

}