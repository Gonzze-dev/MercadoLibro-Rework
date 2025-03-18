using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string Title { get; set; }
        public string? ImageUrl { get; set; }
        public required int OrderID { get; set; }
        public required string ISBN { get; set; }
        public virtual Order? Order { get; set; }
    }
}
