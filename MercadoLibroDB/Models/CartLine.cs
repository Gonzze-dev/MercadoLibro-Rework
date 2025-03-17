using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class CartLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
        //Add isbn forekey when we have the book model
    }
}
