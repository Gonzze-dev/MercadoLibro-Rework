using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercadoLibroDB.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id;

        public required string Token { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool Revoke { get; set; } = false;

        public Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
