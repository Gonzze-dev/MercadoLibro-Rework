namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class SignUpAuthRequest
    {
        public required string Name;
        public required string Email;
        public required string ProviderId;
        public required string AuthMethod;
    }
}
