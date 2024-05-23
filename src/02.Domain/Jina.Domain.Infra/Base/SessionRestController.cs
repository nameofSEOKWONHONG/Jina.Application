using Jina.Domain.Service.Infra.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Domain.Service.Infra;

/// <summary>
/// rest api using session
/// </summary>
[Authorize]
[TypeFilter(typeof(ActionExecuteFilter))]
[Route("api/[controller]")]
public abstract class SessionRestController : JControllerBase
{
    
}

/// <summary>
/// action api using session
/// </summary>
[Authorize]
[TypeFilter(typeof(ActionExecuteFilter))]
[Route("api/[controller]/[action]")]
public abstract class SessionActionController : JControllerBase
{
    
}

/// <summary>
/// action api not use session
/// </summary>
[Route("api/[controller]/[action]")]
public abstract class ActionController : JControllerBase
{
    
}

/// <summary>
/// rest api controller, not use session
/// </summary>
[Route("api/[controller]")]
public abstract class RestController : JControllerBase
{
    
}