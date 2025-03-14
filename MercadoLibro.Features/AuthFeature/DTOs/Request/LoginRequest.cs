using System.ComponentModel.DataAnnotations;

namespace MercadoLibro.Features.AuthFeature.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [MaxLength(50)]
        public required string Password { get; set; }
    }
}
