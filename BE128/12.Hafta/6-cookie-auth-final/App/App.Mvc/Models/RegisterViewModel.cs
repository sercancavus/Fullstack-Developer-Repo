using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required,EmailAddress,MaxLength(150)]
        public string Email { get; set; } = null!;
        [Required,DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string PasswordRepeat { get; set; } = null!;
    }
}
