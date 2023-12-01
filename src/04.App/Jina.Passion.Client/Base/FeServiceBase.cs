namespace Jina.Passion.Client.Base
{
    public class FeServiceBase
    {
        protected HttpClient Client;

        public FeServiceBase(HttpClient client)
        {
            Client = client;
        }
    }
}