using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public decimal Total { get; set; }

        public string? CouponCode { get; set; }

        [Range(0.00, 100.00, ErrorMessage = "Value of discount is between (0.00 - 100)")]
        public decimal DiscountCode { get; set; }

        [Range(0.00, 100.00, ErrorMessage = "Value of discount is between (0.00 - 100)")]
        public decimal DiscountBook{ get; set; }
        public required string UserEmail { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Province { get; set; }
        public required string Country { get; set; }
    }
}
