using System.Text.Json.Serialization;

namespace MercadoLibro.Features.AuthFeature.DTOs
{
    public class SocialPayload
    {
        public required string Email { get; set; }

        public required string Name { get; set; }

        public string AuthMethod { get; set; } = "Google";

        public SocialPayload() { }

        public void SetAuthMethod(
            string email,
            string name,
            string authMethod
        )
        {
            Email = email;
            Name = name;
            AuthMethod = authMethod.ToLower();
        }
    }
}
