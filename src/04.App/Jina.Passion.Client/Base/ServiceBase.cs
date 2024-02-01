using System.Net.Http;

namespace Jina.Passion.Client.Base
{
    public class ServiceBase
    {
        protected HttpClient Client;

        public ServiceBase(HttpClient client)
        {
            Client = client;
        }
    }
}