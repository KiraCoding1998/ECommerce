using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ECommerce.Client.Pages
{
    public partial class Login
    {
        public SharedClientModel Client { get; set; } = new SharedClientModel();
    }
}
