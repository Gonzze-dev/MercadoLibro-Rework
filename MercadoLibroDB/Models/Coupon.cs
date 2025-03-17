using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MercadoLibroDB.Models
{
    public class Coupon
    {
        [Key]
        public required string Code { get; set; }

        [Range(0.00, 100.00, ErrorMessage = "Value of discount is between (0.00 - 100)")]
        [Precision(10,2)]
        public required decimal Discount { get; set; }
        public required bool IsActive { get; set; } = false;
    }
}
