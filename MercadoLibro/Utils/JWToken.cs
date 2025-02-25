using MercadoLibroDB.Models;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MercadoLibro.Utils
{
    public class JWToken(
        IConfiguration configuration
    )
    {
        readonly IConfiguration _configuration = configuration;

        public string GenerateToken(
            UserAuth userAuth
        )
        {
            int hourExpiration = 1;
            SecurityToken token;
            string jwtToken;

            string secret = _configuration["Jwt:Key"] 
                ?? throw new Exception("Secret not found");

            string role = userAuth.Admin ? "Admin" : "User";

            byte[] key = Encoding.ASCII.GetBytes(secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userAuth.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            SecurityTokenDescriptor tokenDescriptor = new ()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(hourExpiration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            JwtSecurityTokenHandler tokenHandler = new();

            token = tokenHandler.CreateToken(tokenDescriptor);
            jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
