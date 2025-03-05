using System.ComponentModel.DataAnnotations;

namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
