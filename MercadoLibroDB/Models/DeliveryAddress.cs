using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Additional { get; set; }
        public int Dni { get; set; }
        public required string Phone { get; set; }
        public Guid UserId { get; set; }
        public required string PostalCode { get; set; }

        [ForeignKey("PostalCode")]
        public virtual City? City { get; set; }
    }
}
