namespace MercadoLibro.Features.genre.DTOs
{
    public class UpdateGenreReq
    {
        public required string OldName { get; set; }
        public required string NewName { get; set; }
    }
}
