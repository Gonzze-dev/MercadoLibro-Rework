using MercadoLibro.Features.AuthFeature.Config;
using MercadoLibro.Features.AuthFeature.DTOs;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using MercadoLibro.Features.General.Utils;

namespace MercadoLibro.Features.AuthFeature.Utils
{
    public class SocialRedHelper
    {
        public List<string> Errors { get; set; } = [];

        public delegate Task<SocialPayload?> SocialAuthMethod(IConfiguration configuration, string idToken);

        public readonly Dictionary<string, SocialAuthMethod> keyValuePairs;

        public SocialRedHelper()
        {
            keyValuePairs = new()
            {
                { "google", GoogleAuthMethod},
                { "x", GoogleAuthMethod},
                { "facebook", FacebookAuthMehtod},
            };
        }

        public async Task<SocialPayload?> GoogleAuthMethod(
            IConfiguration configuration,
            string idToken
        )
        {
            string authMethod = "google";
            string? clientId = configuration["GoogleClientId"];

            string audience;
            GoogleJsonWebSignature.Payload payload;

            SocialPayload socialPayload;

            if (clientId is null)
            {
                Errors.Add("Client id of google is null in the server");
                return null;
            }

            payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            if (payload is null)
            {
                Errors.Add("invalid id token from google");
                return null;
            }

            audience = (string)payload.Audience;

            if (!audience.Contains(clientId))
            {
                Errors.Add("The provided token is not valid for this application.");
                return null;
            }
            //register user
            socialPayload = new()
            {
                Email = payload.Email,
                Name = payload.Name,
                AuthMethod = authMethod.ToLower()
            };

            return socialPayload;
        }

        public async Task<SocialPayload?> FacebookAuthMehtod(
            IConfiguration configuration,
            string accessToken
        )
        {
            SocialPayload? payload;
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var options = JsonSerealOptions.Options;

            var response = await client.GetAsync($"{FacebookConfig.FACEBOOK_AUTH_URL}/me?fields={FacebookConfig.SCOPE}");

            if (!response.IsSuccessStatusCode)
            {
                Errors.Add("Invalid token from facebook");
                return null;
            }

            var content = response.Content.ReadAsStream();

            payload = JsonSerializer.Deserialize<SocialPayload>(content, options);

            if (payload is null)
            {
                Errors.Add("Error parsing facebook response");
                return null;
            }

            return payload;
        }

        public bool HasErrors() =>
              Errors.Count > 0;
        
    }
}
