using Jina.Domain.Service.Infra.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Domain.Service.Infra;

[TypeFilter(typeof(ActionExecuteFilter))]
[Authorize]
public abstract class SessionController : JControllerBase
{
    
}