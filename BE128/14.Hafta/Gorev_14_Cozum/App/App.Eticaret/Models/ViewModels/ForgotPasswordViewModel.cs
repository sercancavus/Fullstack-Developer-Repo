using System.ComponentModel.DataAnnotations;

namespace App.Eticaret.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = null!;
    }
}