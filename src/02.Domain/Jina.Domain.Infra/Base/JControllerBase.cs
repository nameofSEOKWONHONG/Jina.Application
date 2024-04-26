using eXtensionSharp;
using Jina.Base.Service;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Jina.Domain.Service.Infra;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class JControllerBase : ControllerBase
{
    protected ILogger Logger => Log.Logger;
    private ServicePipeline _spl;
    private ISessionContext _ctx;

    protected ServicePipeline Spl
    {
        get
        {
            if (_spl.xIsEmpty()) _spl = this.HttpContext.RequestServices.GetRequiredService<ServicePipeline>();
            return _spl;
        }
    }

    protected ISessionContext Ctx
    {
        get
        {
            if (_ctx.xIsEmpty()) _ctx = this.HttpContext.RequestServices.GetRequiredService<ISessionContext>();
            return _ctx;
        }
    }

    protected JControllerBase()
    {
    }
}