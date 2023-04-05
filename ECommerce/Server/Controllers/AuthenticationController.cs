using ECommerce.Server.Services;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IClientService clientService , ITokenService tokenService)
        {
            _clientService =  clientService;
            _tokenService = tokenService;
        }


        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateClient(SharedClientModel loginClient)
        {
            var result = await _clientService.GetClientAsync(loginClient);

            if (result == null)
            {
                return Unauthorized();  
            }

            var response = new AuthenticationResponse
            {
                Token = _tokenService.GenerateToken(result)
            };
            return Ok(response);
        }

        /// <summary>
        /// This api Adds a client and authenticate them.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(SharedClientModel client)
        {
            // IsAdmin is always false here so no need to create a registration model.
            //There's Also a need to create a Registration Response Model to display a message for the error 

            var usernames = await _clientService.GetUsernames();

            if(usernames.Contains(client.Username))
            {
                return BadRequest("Userame already exists!");
            }

            var record = await _clientService.AddClientAsync(client);

            

            if(record == null)
            {
                
                return BadRequest("");
            }

            var response = new AuthenticationResponse
            {
                Token = _tokenService.GenerateToken(record)
            };

            return Ok(response);
        }


    }
}
