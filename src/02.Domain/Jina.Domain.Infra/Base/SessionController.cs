using Jina.Domain.Service.Infra.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Domain.Service.Infra;

[Authorize]
[TypeFilter(typeof(ActionExecuteFilter))]
public abstract class SessionController : JControllerBase
{
    
}