using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MercadoLibroDB.Models
{
    [method: SetsRequiredMembers]
    public class User(
        string name,
        string email
        )
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; } = name;

        [EmailAddress]
        public required string Email { get; set; } = email;
        public bool Admin { get; set; } = false;

    }
}
