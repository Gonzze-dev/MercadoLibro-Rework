namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class SignUpRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}
