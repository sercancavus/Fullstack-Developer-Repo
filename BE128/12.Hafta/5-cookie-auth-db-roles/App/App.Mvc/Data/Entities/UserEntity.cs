using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Mvc.Data.Entities
{
    public class UserEntity
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!; // null olamaz
        [Required, MaxLength(150)]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;

        // -----------------------------

        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public RoleEntity Role { get; set; } = default!;

    }
}
