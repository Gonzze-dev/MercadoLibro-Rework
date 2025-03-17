namespace MercadoLibroDB.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
