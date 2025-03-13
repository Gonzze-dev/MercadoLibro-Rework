namespace MercadoLibro.Features.General.DTOs
{
    public class ErrorHttp
    {
        public string Description { get; set; }
        public int StatusCode { get; set; }

        public ErrorHttp(
            string description,
            int statusCode
        )
        {
            Description = description;
            StatusCode = statusCode;
        }
    }
}
