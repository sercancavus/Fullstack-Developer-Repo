using System.ComponentModel.DataAnnotations;

namespace App.Eticaret.Models.ViewModels
{
    public class RenewPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MinLength(1)]
        public string Token { get; set; } = default!;

        [Required, MinLength(1)]
        public string Password { get; set; } = default!;

        [Required, MinLength(1), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
