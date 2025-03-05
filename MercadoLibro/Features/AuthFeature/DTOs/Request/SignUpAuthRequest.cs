using System.ComponentModel.DataAnnotations;

namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class SignUpAuthRequest
    {
        [Required]
        public required string Name;
        [Required]
        [EmailAddress]
        public required string Email;
        [Required]
        public required string ProviderId;
        [Required]
        public required string AuthMethod;
    }
}
