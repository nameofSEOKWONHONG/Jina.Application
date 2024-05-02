using System.Net.Http;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Infra;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jina.Passion.Client.Base
{
    public abstract class ServiceBase
    {
        protected readonly IRestClient Client;
        protected readonly ISessionStorageService SessionStorageService;
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

        protected ServiceBase(IRestClient client, ISessionStorageService sessionStorageService) : this(client)
        {
            SessionStorageService = sessionStorageService;
        }

        protected ServiceBase(IRestClient client, ISessionStorageService sessionStorageService,
            AuthenticationStateProvider authenticationStateProvider) : this(client, sessionStorageService)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }
        
    }
}