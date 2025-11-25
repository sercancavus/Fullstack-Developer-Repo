using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, MinLength(6), MaxLength(200)]
        public string Password { get; set; } = string.Empty; // TODO: Hash storage
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public bool Enabled { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}