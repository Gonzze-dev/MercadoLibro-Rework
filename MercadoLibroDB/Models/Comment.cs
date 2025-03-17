using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MercadoLibroDB.Models
{
    public class Comment
    {
        [MaxLength(4000, ErrorMessage = "Max length of coment is 4000")]
        [MinLength(1, ErrorMessage = "Min length of coment is 1")]
        public required string UserComment { get; set; }
        public Guid UserID { get; set; }
        public required string ISBN { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        [ForeignKey("ISBN")]
        public virtual Book? Book { get; set; }
    }
}
