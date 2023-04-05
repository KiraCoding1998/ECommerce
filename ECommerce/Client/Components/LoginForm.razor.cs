using ECommerce.Client.Classes;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace ECommerce.Client.Components
{
    public partial class LoginForm
    {
        [Parameter]
        public SharedClientModel Client { get; set; } = new SharedClientModel();

        [Inject]
        public HttpClient? Http { get; set; }

        [Inject]
        public AuthenticationStateProvider? AuthenticationState { get; set; }

        [Inject]
        public NavigationManager? Navigator { get;set ; }

        [Inject]
        public IJSRuntime? JS { get; set ; }


        /// <summary>
        /// Generates a post request to Authentication controller and checks if use exists
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            var response = await Http?.PostAsJsonAsync("api/Authentication/authenticate", Client!)!;

            if(response.IsSuccessStatusCode)
            {
                AuthenticationResponse? authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

                var customAuthProvider = (CustomAuthenticationStateProvider?) AuthenticationState;

                if(customAuthProvider != null)
                {
                   await customAuthProvider.SetAuthenticationStateAsync("token", authResponse!.Token);
                }

                Navigator?.NavigateTo("/home");
            }

            else
            {
                 JS?.InvokeVoidAsync("alert", "You are not authorized");
            }

        }

        private void NavigateToRegister()
        {
            Navigator?.NavigateTo("/register");
        }
    }
}
