using System.ComponentModel.DataAnnotations;

namespace App.Admin.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(4)]
        public string Password { get; set; } = null!;
    }
}