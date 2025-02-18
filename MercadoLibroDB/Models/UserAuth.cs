using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MercadoLibroDB.Models
{
    public class UserAuth
    {

        [Key]
        public Guid Id { get; set; }
        public required string Password { get; set; }
        public bool Admin { get; set; } = false;
        public Guid? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }


        public UserAuth() { }

        [SetsRequiredMembers]
        public UserAuth(string password)
        {
            Password = password;
            Admin = false;
        }
    }
}
