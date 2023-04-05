using Blazored.SessionStorage;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.Client.Classes
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly HttpClient _httpClient;


        //if it's empty then the user is not authenticated
        private ClaimsPrincipal _anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService, HttpClient httpClient)
        {
            _sessionStorageService = sessionStorageService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userToken = await _sessionStorageService.GetItemAsStringAsync("token");

            if(userToken == null || string.IsNullOrEmpty(userToken))
            {
                return new AuthenticationState(_anonymousUser);
            }

            //create a claim prinicipal for the valid client
            ClaimsPrincipal claimingUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(userToken) , "jwtAuth"));
            
            //inject the token into the http request to check for authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

            return new AuthenticationState(claimingUser);
        }

        
        public async Task SetAuthenticationStateAsync(string key , string value)
        {
           await _sessionStorageService.SetItemAsStringAsync(key, value);
           NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task RemoveAuthenticationState(string key)
        {
            await _sessionStorageService.RemoveItemAsync("token");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }



        public void UpdateAuthenticationState(ClaimsIdentity claimsIdentity)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }


        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
