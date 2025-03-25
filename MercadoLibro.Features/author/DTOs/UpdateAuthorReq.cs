namespace MercadoLibro.Features.author.DTOs
{
    public class UpdateAuthorReq
    {
        public required string OldName { get; set; }
        public required string NewName { get; set; }
    }
}
