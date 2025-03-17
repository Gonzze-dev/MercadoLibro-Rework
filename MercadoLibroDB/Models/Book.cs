using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class Book
    {
        public required string ISBN { get; set; }
        public required string ImageUrl{ get; set; }
        public required string Title { get; set; }
        public required DateOnly EditionDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The stock must be positive")]
        public required int Stock { get; set; }
        public string? Description { get; set; }
        public required DateOnly EntryDate { get; set; }

        [Range(0.00, 100, ErrorMessage = "Value of discount is between (0.00 - 100)")]
        public required decimal Discount { get; set; }
        public int PublisherId { get; set; }
        public int LanguageId { get; set; }

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }
        
        [ForeignKey("LanguageId")]
        public virtual Language? Language { get; set; }

        public virtual ICollection<Author>? Authors { get; set; }

        public virtual ICollection<Genre>? Genre { get; set; }
    }
}
