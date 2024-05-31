using Toolbelt.Blazor;

namespace Jina.Passion.Client.Share.Common;

public interface IHttpInterceptorManager
{
    void RegisterEvent();

    Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

    void DisposeEvent();
}