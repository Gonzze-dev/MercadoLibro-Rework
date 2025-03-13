using System.ComponentModel.DataAnnotations;

namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class SignUpSocialRequest
    {
        [Required]
        public required string IdToken { get; set; }

        [Required]
        public required string AuthMethod { get; set; }
    }
}
