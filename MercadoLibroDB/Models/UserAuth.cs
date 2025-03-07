using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MercadoLibroDB.Models
{
    public class UserAuth
    {

        [Key]
        public Guid Id { get; set; }
        public string? Password { get; set; }
        
        public string AuthMethod {get; set;} = "local";

        public Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
