using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ECommerce.Client.Components
{
    public partial class ProductCard
    {
        [Parameter]
        public ProductDto Product { get; set; } = default!;

        [Parameter]
        public EventCallback OnAddToCart { get; set; }

        private async Task AddToCartEventInvoker()
        {
            await OnAddToCart.InvokeAsync();
        }
    }
}
