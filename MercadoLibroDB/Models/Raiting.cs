using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class Raiting
    {
        [Range(0,5, ErrorMessage = "Value of raiting is between (0 - 5)")]
        public int RaitingBook { get; set; }
        public Guid UserID { get; set; }
        public required string ISBN { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        [ForeignKey("ISBN")]
        public virtual Book? Book { get; set; }
    }
}
