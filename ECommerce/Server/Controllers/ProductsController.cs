using AutoMapper;
using ECommerce.Server.Models;
using ECommerce.Server.Services;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addproduct")]
        [Authorize(Roles = "True")]
        public async Task<ActionResult<ProductDto>> AddProduct(ProductDto product)
        {
            var resultObject = await _productService.AddProductAsync(product);

            if(resultObject != null)
            {
                product.Id = resultObject.Id;
                return Ok(product);
            }

            return BadRequest("An error occured while performing this operation!");
        }

        [AllowAnonymous]
        [HttpGet("getallproducts")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }

        [HttpDelete("deleteproduct/{id}")]
        [Authorize(Roles = "True")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            bool result = await _productService.DeleteProductAsync(id); 
            if(result)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }


        [HttpPost("updateproduct")]
        [Authorize(Roles ="True")]
        public async Task<IActionResult> UpdateProduct(ProductDto product)
        {
            var result = await _productService.UpdateProductAsync(product);

            if(result)
            {
                return Ok("item updated successfuly");
            }
            return BadRequest("Failed to update item");
        }

        [AllowAnonymous]
        [HttpGet("getAvailableProducts")]
        public async Task<ActionResult<List<ProductDto>>> GetAvailableProducts()
        {
            var availableProducts = await _productService.GetAvailableProductsAsync();
            return Ok(availableProducts);
        }

        [HttpGet("searchproduct/{searchValue}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> SearchProduct(string? searchValue)
        {
            var products = await _productService.SearchProducts(searchValue!);
            return Ok(products);
        }

        [HttpPost("addAllproducts")]
        [Authorize(Roles ="True")]
        public async Task<ActionResult<List<ProductDto>>> AddAllProducts(List<ProductDto> products)
        {
            var failed = await  _productService.AddAllProductsAsync(products);
            return Ok(failed);
        }

        [HttpGet("getsomeproducts")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> GetSomeproducts()
        {
            return Ok(await _productService.GetSomeProductsAsync());
        }
    }
}
