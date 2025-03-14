using System.Security.Cryptography;

namespace MercadoLibro.Features.RefreshTokenFeature
{
    public class RefreshTokenHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var ranNumGenerator = RandomNumberGenerator.Create();
            ranNumGenerator.GetBytes(randomBytes);
            string refreshToken = Convert.ToBase64String(randomBytes);

            return refreshToken;
        }
    }
}
