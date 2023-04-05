using ECommerce.Client.Classes;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace ECommerce.Client.Components
{
    public partial class RegistrationForm
    {
        [Parameter]
        public SharedClientModel Client { get; set; } = new SharedClientModel();

        [Inject]
        public HttpClient? Http { get; set; }

        [Inject]
        public AuthenticationStateProvider? AuthenticationState { get; set; }

        [Inject]
        public NavigationManager? Navigator { get; set; }

        [Inject]
        public IJSRuntime? JS { get; set; }

        private string usernameExistsError = string.Empty;


        /// <summary>
        /// Generates a post request to Authentication controller and checks if use exists
        /// </summary>
        /// <returns></returns>
        public async Task Register()
        {

            var response = await Http?.PostAsJsonAsync("api/Authentication/register", Client!)!;

            if (response.IsSuccessStatusCode)
            {
                AuthenticationResponse? authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

                var customAuthProvider = (CustomAuthenticationStateProvider?)AuthenticationState;

                if (customAuthProvider != null)
                {
                    await customAuthProvider.SetAuthenticationStateAsync("token", authResponse!.Token);
                }

                Navigator?.NavigateTo("/home");
            }

            else
            {
                var error = await response.Content.ReadAsStringAsync();

                if(error.Equals(""))
                {
                    JS?.InvokeVoidAsync("alert", "An error occured, please try again");
                }

                else
                {
                    usernameExistsError =  error;
                }
            }

        }


        private void NavigateToLogin()
        {
            Navigator?.NavigateTo("/login");
        }

        public void Clear()
        {
            usernameExistsError = string.Empty;
        }
    }
}