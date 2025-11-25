using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required,MinLength(2)]
        public string Name { get; set; } = null!;
        [Required, MinLength(2)]
        public string Surname { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password), MinLength(4)]
        public string Password { get; set; } = null!;
        [Required, Compare(nameof(Password))]
        public string PasswordRepeat { get; set; } = null!;

    }
}
