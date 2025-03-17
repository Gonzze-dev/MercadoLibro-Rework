using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? CouponCode { get; set; }

        public Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        public virtual ICollection<CartLine>? CartLine { get; set; }

    }


}
