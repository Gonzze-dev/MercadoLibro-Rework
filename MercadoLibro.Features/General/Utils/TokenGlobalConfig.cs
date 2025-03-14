namespace MercadoLibro.Features.General.Utils
{
    public class TokenGlobalConfig
    {
        readonly static int JwtLife = 1; // hours
        readonly static int RefreshTokenLife = 7; // days

        public static DateTime GetLifeToken() => DateTime.UtcNow.AddHours(JwtLife);

        public static DateTime GetRefreshTokenLife() => DateTime.UtcNow.AddDays(RefreshTokenLife);
    }
}
