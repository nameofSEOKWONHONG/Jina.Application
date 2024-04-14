using System.Net.Http;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Infra;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jina.Passion.Client.Base
{
    public abstract class ServiceBase
    {
        protected readonly IRestClient Client;
        protected readonly ISessionStorageHandler SessionStorageHandler;
        protected readonly AuthenticationStateProvider AuthenticationStateProvider;
        //TODO : localstorage 추가

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected ServiceBase(IRestClient client)
        {
            Client = client;
        }

        protected ServiceBase(IRestClient client, ISessionStorageHandler sessionStorageHandler) : this(client)
        {
            SessionStorageHandler = sessionStorageHandler;
        }

        protected ServiceBase(IRestClient client, ISessionStorageHandler sessionStorageHandler,
            AuthenticationStateProvider authenticationStateProvider) : this(client, sessionStorageHandler)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }
        
    }
}