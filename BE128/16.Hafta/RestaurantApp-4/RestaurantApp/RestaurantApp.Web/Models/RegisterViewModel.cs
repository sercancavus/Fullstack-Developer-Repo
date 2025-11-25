using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
