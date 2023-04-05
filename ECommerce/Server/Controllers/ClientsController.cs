using AutoMapper;
using ECommerce.Server.Services;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;

namespace ECommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientsController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("getclientproducts")]
        [Authorize(Roles = "False")]
        public async Task<ActionResult<Models.Client>> GetClientProducts()
        {
           var products =  await _clientService.GetCurrentClientProductsAsync();

            return Ok(products);
        }


        [HttpPost("addtoCart")]
        [Authorize(Roles = "False")]
        public async Task<IActionResult> AddtoCart(ProductDto product)
        {
            var success = await _clientService.AddProductToClientAsync(product);

            if(success)
            {
                return Ok("operation Succeeded");
            }

            return BadRequest("Failed to perfrom operation");
        }
    }
}
