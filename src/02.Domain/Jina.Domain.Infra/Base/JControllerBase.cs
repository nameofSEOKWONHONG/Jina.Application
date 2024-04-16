using eXtensionSharp;
using Jina.Base.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Jina.Domain.Service.Infra;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class JControllerBase : ControllerBase
{
    protected ILogger Logger => Log.Logger;
    private ServicePipeline _svc;

    protected ServicePipeline Pip
    {
        get
        {
            if (_svc.xIsEmpty()) _svc = this.HttpContext.RequestServices.GetRequiredService<ServicePipeline>();
            return _svc;
        }
    }

    protected JControllerBase()
    {
    }
}

[Authorize]
public abstract class SessionController : JControllerBase
{
    
}