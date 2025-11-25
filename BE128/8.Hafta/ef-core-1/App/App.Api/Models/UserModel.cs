using System.ComponentModel.DataAnnotations;

namespace App.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty; // Default Value

        [Required, StringLength(50, MinimumLength = 1), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(4)]
        public string Password { get; set; } = string.Empty;
    }
}
