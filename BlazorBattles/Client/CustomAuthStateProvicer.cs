using BlazorBattles.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client
{
    public class CustomAuthStateProvicer : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;
        private readonly IBananaService _bananaService;

        public CustomAuthStateProvicer(ILocalStorageService localStorageService, HttpClient http,
            IBananaService bananaService)
        {
            _localStorageService = localStorageService;
            _http = http;
            _bananaService = bananaService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");


            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                    await _bananaService.GetBananas();

                }
                catch (Exception)
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                                    }
                        }
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);              
            

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string payload)
        {
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
                default:
                    break;
            }
            return Convert.FromBase64String(payload);
        }
    }
}
