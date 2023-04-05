using AutoMapper;
using ECommerce.Server.Data;
using ECommerce.Server.Helpers;
using ECommerce.Server.Models;
using ECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace ECommerce.Server.Services
{
    public class ClientService : IClientService
    {

        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;


        public ClientService(DataContext dataContext, IHttpContextAccessor httpContext , IMapper mapper)
        {
            _dataContext = dataContext;
            _contextAccessor = httpContext;
            _mapper = mapper;
        }

        public async Task<List<Models.Client>> GetClientsAsync()
        {
            return await _dataContext.Clients.ToListAsync();
        }


        public Task<HashSet<string>> GetUsernames()
        {
            return Task.FromResult(_dataContext.Clients.Select(c => c.Username).ToHashSet());
        }


        
        public async Task<Models.Client?> AddClientAsync(SharedClientModel loginClient)
        {
            PasswordManager passwordObject = PasswordManager.CreatePassowrdObject(loginClient.Password);

            Models.Client client = new Models.Client
            {
                IsAdmin = loginClient.IsAdmin,
                Username = loginClient.Username,
                PasswordHash = passwordObject.HashedPassword,
                PasswordSalt = passwordObject.PasswordSalt
            };

            var record = await _dataContext.Clients.AddAsync(client);
            if(await _dataContext.SaveChangesAsync() > 0)
            {
                return record.Entity;
            }

            return null;
        }

        public async Task<Models.Client?> GetClientAsync(SharedClientModel client)
        {
            //find the username(username is unique)
            var record = await _dataContext.Clients.FirstOrDefaultAsync(x => x.Username == client.Username);

            if(record != null) 
            {
                if (PasswordManager.IsPasswordVerified(client.Password, record.PasswordSalt!, record.PasswordHash!))
                {
                    return record;
                }
            }

            return null;

        }


        public async Task<bool> AddProductToClientAsync(ProductDto product)
        {

            int userId = int.Parse(_contextAccessor?.HttpContext?.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)!);

            var clientProduct = await _dataContext.ClientProducts
                .FirstOrDefaultAsync(x => x.ProductId == product.Id && x.ClientId == userId);

            //if client already has this product
            if (clientProduct != null)
            {
                clientProduct!.ClientHas++;
                _dataContext.ClientProducts.Update(clientProduct);
            }

            //if client newly buying this product
            else
            {
                //search for the client
                var client = await _dataContext.Clients.FirstOrDefaultAsync(c => c.Id == userId);

                //create new entity of ClientProduct
                var newClientProduct = new ClientProduct
                {
                    Client = client,
                    ProductId = product.Id,
                    ClientHas = 1
                };


                await _dataContext.ClientProducts.AddAsync(newClientProduct);
            }

            //update the product (update the pieces available only , can use Attach() too)
          var affected = await _dataContext.Products.Where(p => p.Id == product.Id).ExecuteUpdateAsync(
                o => o.SetProperty(p => p.PiecesAvaliable , p => p.PiecesAvaliable - 1));


            return await _dataContext.SaveChangesAsync() > 0 && affected > 0;
        }

        public async Task<List<OwnedProductModel?>?> GetCurrentClientProductsAsync()
        {
            //get user from the http context
            int clientId = int.Parse( _contextAccessor?.HttpContext?.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)!);

            var products = await _dataContext.ClientProducts
                                    .Where(cp => cp.ClientId == clientId)
                                    .Select(cp => new OwnedProductModel
                                    {
                                        ProductDescription = cp.Product.Description,
                                        ProductName = cp.Product.ProductName,
                                        ProductPrice = cp.Product.Price,
                                        ClientHas = cp.ClientHas,
                                        ProductImage = cp.Product.ProductImage
                                    }).ToListAsync();


            return products;
        }
    }
}
