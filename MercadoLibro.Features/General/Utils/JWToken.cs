using MercadoLibroDB.Models;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace MercadoLibro.Features.General.Utils
{
    public class JWToken(
        IConfiguration configuration
    )
    {
        readonly IConfiguration _configuration = configuration;

        public string GenerateToken(
            UserAuth userAuth,
            User user
        )
        {
            SecurityToken token;
            string jwtToken;

            byte[] keyEncoding;
            SymmetricSecurityKey key;
            SigningCredentials creds;

            string secret = _configuration["Jwt:Key"]
                ?? throw new Exception("Secret not found");

            string role = user.Admin ? "Admin" : "User";

            keyEncoding = Encoding.ASCII.GetBytes(secret);
            key = new(keyEncoding);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, userAuth.AuthMethod),
                new Claim(ClaimTypes.Role, role)
            };

            creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = TokenGlobalConfig.GetLifeToken(),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new();

            token = tokenHandler.CreateToken(tokenDescriptor);
            jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
