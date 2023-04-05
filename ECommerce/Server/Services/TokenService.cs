using ECommerce.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Create the JWt web token incase the user is logged in
        /// </summary>
        /// <param name="clientDto"></param>
        /// <returns></returns>
        public string GenerateToken(Models.Client client)
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , client.Id.ToString()),
                new Claim(ClaimTypes.Name , client.Username),
                new Claim(ClaimTypes.Role , client.IsAdmin.ToString())
            };

            string jwtKey = _configuration.GetSection("JwtKey:SecretKey").Value!;
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var cred = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(claims: userClaims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtToken;
        }


        public string GetViewUserToken(Models.Client client)
        {
            List <Claim>  viewUserClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , client.Username),
                new Claim(ClaimTypes.Role , "ViewUser")
            };

            string jwtKey = _configuration.GetSection("JwtKey:SecretKey").Value!;
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var cred = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(claims: viewUserClaims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtToken;

        }

        public string GetAdminToken(Models.Client client)
        {
            List<Claim> viewUserClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , client.Username),
                new Claim(ClaimTypes.Role , "True")
            };

            string jwtKey = _configuration.GetSection("JwtKey:SecretKey").Value!;
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var cred = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(claims: viewUserClaims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtToken;

        }



    }
}
