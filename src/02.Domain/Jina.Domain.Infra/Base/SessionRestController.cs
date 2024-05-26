using Jina.Domain.Service.Infra.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Domain.Service.Infra;

/// <summary>
/// action api not use session
/// </summary>
[TypeFilter<ActionExecuteFilter>]
[Route("api/[controller]/[action]")]
public abstract class ActionController : JControllerBase
{
    
}

/// <summary>
/// rest api controller, not use session
/// </summary>
[TypeFilter<ActionExecuteFilter>]
[Route("api/[controller]")]
public abstract class RestController : JControllerBase
{
    
}

/// <summary>
/// rest api using session
/// </summary>
[Authorize]
[TypeFilter<SessionExecuteFilter>]
[Route("api/[controller]")]
public abstract class SessionRestController : RestController
{
    
}

/// <summary>
/// action api using session
/// </summary>
[Authorize]
[TypeFilter<SessionExecuteFilter>]
[Route("api/[controller]/[action]")]
public abstract class SessionActionController : ActionController
{
    
}