using Jina.Base.Service;

namespace Jina.Domain.Service.Infra;

/// <summary>
/// Not support session context, If you need information about the session, pass it as a parameter.
/// </summary>
public abstract class JobBase
{
    protected ServicePipeline Spl;

    protected JobBase(ServicePipeline spl)
    {
        this.Spl = spl;
    }
}