using Toolbelt.Blazor;

namespace Jina.Passion.Client.Common.Infra
{
    public interface IHttpInterceptorManager
    {
        void RegisterEvent();

        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

        void DisposeEvent();
    }
}
