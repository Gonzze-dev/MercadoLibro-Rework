namespace MercadoLibro.Features.AuthFeature.DTOs.Service
{
    public class TokenResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
