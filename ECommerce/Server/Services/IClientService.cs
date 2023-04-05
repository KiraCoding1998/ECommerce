using ECommerce.Shared.Models;

namespace ECommerce.Server.Services
{
    public interface IClientService
    {
        Task<Models.Client?> GetClientAsync(SharedClientModel loginClient);
        Task<Models.Client?> AddClientAsync(SharedClientModel loginClient);
        Task<HashSet<string>> GetUsernames();
        //the client id can be taken from the http Context accessor
        Task<List<OwnedProductModel?>?> GetCurrentClientProductsAsync();
        Task<bool> AddProductToClientAsync(ProductDto product);
    }
}
