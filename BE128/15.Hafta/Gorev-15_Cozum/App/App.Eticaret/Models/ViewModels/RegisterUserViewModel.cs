using System.ComponentModel.DataAnnotations;

namespace App.Eticaret.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, MinLength(4), DataType(DataType.Password), Compare(nameof(Password))]
        public string PasswordConfirm { get; set; } = null!;
    }
}