﻿using System.Security.Claims;
using System.Text.Json;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
using Jina.Passion.Client.Share.Consts;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jina.Passion.Client.Share.Common;

public class AuthenticationStateProviderImpl : AuthenticationStateProvider
    {
        public ClaimsPrincipal AuthenticationStateUser { get; set; }
        public string Token { get; private set; }

        private readonly ISessionStorageWrapperService _sessionStorageWrapperService;

        public AuthenticationStateProviderImpl(ISessionStorageWrapperService stateWrapperService)
        {
            _sessionStorageWrapperService = stateWrapperService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                Token = await _sessionStorageWrapperService.GetAsync(StorageConsts.Local.AuthToken);
                if (Token.xIsEmpty())
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(Token), "jwt")));
            AuthenticationStateUser = state.User;

            return state;
        }

        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles.xIsNotEmpty())
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
                if (permissions.xIsNotEmpty())
                {
                    if (permissions.ToString().Trim().StartsWith("["))
                    {
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                        claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                    }
                    else
                    {
                        claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                    }
                    keyValuePairs.Remove(ApplicationClaimTypes.Permission);
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string payload)
        {
            payload = payload.Trim().Replace('-', '+').Replace('_', '/');
            var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64);
        }

        public async Task StateChangedAsync()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            var state = await GetAuthenticationStateAsync();
            var authenticationStateProviderUser = state.User;
            return authenticationStateProviderUser;
        }
    }