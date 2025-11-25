using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, MinLength(6), MaxLength(200)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}