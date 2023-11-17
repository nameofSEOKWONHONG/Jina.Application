using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Jina.Domain.Infra.Base;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class JControllerBase : ControllerBase
{
    protected ILogger Logger => Log.Logger;
    
    protected JControllerBase()
    {
        
    }
}