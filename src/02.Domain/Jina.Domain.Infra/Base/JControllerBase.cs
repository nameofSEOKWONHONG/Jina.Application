using eXtensionSharp;
using Jina.Base.Service;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Jina.Domain.Service.Infra;

[ApiController]
public abstract class JControllerBase : ControllerBase
{
    protected ILogger Logger => Log.Logger;
    private ServicePipeline _pipe;
    private ISessionContext _context;

    protected ServicePipeline Pipe
    {
        get
        {
            if (_pipe.xIsEmpty()) _pipe = this.HttpContext.RequestServices.GetRequiredService<ServicePipeline>();
            return _pipe;
        }
    }

    protected ISessionContext Context
    {
        get
        {
            if (_context.xIsEmpty()) _context = this.HttpContext.RequestServices.GetRequiredService<ISessionContext>();
            return _context;
        }
    }

    protected JControllerBase()
    {
    }
}