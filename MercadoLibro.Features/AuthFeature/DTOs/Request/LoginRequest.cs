using System.ComponentModel.DataAnnotations;

namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }
}
