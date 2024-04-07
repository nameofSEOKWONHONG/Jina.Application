using Jina.Passion.Client.Common.Infra;

namespace Jina.Passion.Client.Services
{
    public abstract class ServiceBase
    {
        protected readonly IRestClient Client;

        public ServiceBase(IRestClient client)
        {
            Client = client;
        }
    }
}
