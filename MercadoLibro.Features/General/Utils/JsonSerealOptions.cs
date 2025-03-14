using System.Text.Json;
using System.Text.Json.Serialization;

namespace MercadoLibro.Features.General.Utils
{
    public static class JsonSerealOptions
    {
        public static JsonSerializerOptions Options { get; } = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        


    }
}
