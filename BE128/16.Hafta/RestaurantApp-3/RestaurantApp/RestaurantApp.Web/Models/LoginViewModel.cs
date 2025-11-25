using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
