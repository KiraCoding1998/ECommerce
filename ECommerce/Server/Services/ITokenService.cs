
using ECommerce.Shared.Models;

namespace ECommerce.Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(Models.Client client);
        string GetViewUserToken(Models.Client client);
        string GetAdminToken(Models.Client client);
    }
}
