namespace MercadoLibro.Features.AuthFeature.DTOs
{
    public class CookieConfig()
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public required CookieOptions Options { get; set; }
    }
}
