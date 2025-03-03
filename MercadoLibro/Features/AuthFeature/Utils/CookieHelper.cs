using MercadoLibro.Features.AuthFeature.DTOs;
using MercadoLibro.Utils;

namespace MercadoLibro.Features.AuthFeature.Utils
{
    public class CookieHelper
    {
        public static CookieConfig CreateTokenCookie(string token)
        {
            string key = "access_token";

            CookieOptions options = new()
            {
                SameSite = SameSiteMode.Strict,
                Expires = TokenGlobalConfig.GetRefreshTokenLife(),
            };

            CookieConfig cookie = new()
            {
                Key = key,
                Value = token,
                Options = options
            };

            return cookie;
        }

        public static CookieConfig CreateRefreshTokenCookie(string refreshToken)
        {
            string key = "refresh_token";

            CookieOptions options = new()
            {
                SameSite = SameSiteMode.Strict,
                Expires = TokenGlobalConfig.GetRefreshTokenLife(),
            };

            CookieConfig cookie = new()
            { 
                Key = key, 
                Value = refreshToken,
                Options = options
            };

            return cookie;
        }
    }
}
