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
        public required bool Admin { get; set; }
        public required Guid UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual required User User { get; set; }


        public UserAuth() { }


        [SetsRequiredMembers]
        public UserAuth(Guid userId, string password)
        {
            UserID = userId;
            Password = password;
            Admin = false;
        }
    }
}
