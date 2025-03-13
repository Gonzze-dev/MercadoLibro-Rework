namespace MercadoLibro.Features.General.DTOs
{
    public class GeneralResponse<T>
    {
        public T? Data { get; set; }

        public List<ErrorHttp> Errors { get; set; }

        public GeneralResponse()
        {
            Errors = new();
        }
    }
}
