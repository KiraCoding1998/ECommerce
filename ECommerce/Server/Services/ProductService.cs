using AutoMapper;
using ECommerce.Server.Data;
using ECommerce.Server.Models;
using ECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Extensions;

namespace ECommerce.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product?> AddProductAsync(ProductDto productDto)
        {
            var productModel = _mapper.Map<Product>(productDto);

            var resultObject = await _context.Products.AddAsync(productModel);

            if (await _context.SaveChangesAsync() > 0)
            {
                return resultObject.Entity;
            }

            return null;

        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            Product product = new Product
            {
                Id = id
            };
            var result = _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products =  await _context.Products.ToListAsync();
            return products;

        }

        public async Task<List<ProductDto>> SearchProducts(string searchValue)
        {
            searchValue = searchValue.ToLower();

            var products =  await _context.Products.Where(p => p.ProductName.ToLower().Contains(searchValue)).ToListAsync();


            List<ProductDto> productDtos = new List<ProductDto>();

            Parallel.ForEach(products, product =>
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDtos.Add(productDto);
            });

            return productDtos;
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            _context.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }




        /// <summary>
        /// This function takes a list of products and return the Products that failed to be Added
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<List<ProductDto?>> AddAllProductsAsync(List<ProductDto> products)
        {
            List<ProductDto?> failedTobeAdded = new List<ProductDto?>();

            //var productsModelList = _mapper.Map<List<Product>>(products);

            var productsModelList = new List<Product>();

            Parallel.ForEach(products, p =>
            {
                var pruductModel = _mapper.Map<Product>(p);
                productsModelList.Add(pruductModel);
            });

            await _context.Products.AddRangeAsync(productsModelList);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                failedTobeAdded = ex.Entries
                                   .Where(e => e.State == EntityState.Added)
                                   .Select(e => e.Entity as ProductDto).ToList();

                return failedTobeAdded;
            }

            return failedTobeAdded;

        }

        public async Task<List<ProductDto>> GetAvailableProductsAsync()
        {
            var products =  await _context.Products
                                            .Where(p => p.PiecesAvaliable > 0)
                                            .ToListAsync();

            List<ProductDto> productDtos = new List<ProductDto>();
            Parallel.ForEach(products, p =>
            {
                var productDto = _mapper.Map<ProductDto>(p);
                productDtos.Add(productDto);
            });

            return productDtos;
        }

        public async Task<List<ProductDto>> GetSomeProductsAsync()
        {
            var someProducts = await _context.Products.Take(25).Where(p => p.PiecesAvaliable > 0).ToListAsync();

            return _mapper.Map<List<ProductDto>>(someProducts);
        }
    }
}
