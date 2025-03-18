using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class DeliveryAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Additional { get; set; }
        public required int Dni { get; set; }
        public required string Phone { get; set; }
        public required Guid UserId { get; set; }
        public required int PostalCode { get; set; }

        [ForeignKey("PostalCode")]
        public virtual City? City { get; set; }
    }
}
