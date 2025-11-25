using System.ComponentModel.DataAnnotations;

namespace BlogApp.Mvc.Models
{
    public class LoginViewModel
    {
        [Required, MinLength(4), MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required, MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
