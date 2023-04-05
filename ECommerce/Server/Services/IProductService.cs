using ECommerce.Server.Models;
using ECommerce.Shared.Models;

namespace ECommerce.Server.Services
{
    public interface IProductService
    {
        Task<Product?> AddProductAsync(ProductDto model); 
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProductAsync(ProductDto model);
        Task<List<ProductDto>> SearchProducts(string searchValue);
        Task<List<ProductDto?>> AddAllProductsAsync(List<ProductDto> products);
        Task<List<ProductDto>> GetAvailableProductsAsync();

        Task<List<ProductDto>> GetSomeProductsAsync();
    }
}
