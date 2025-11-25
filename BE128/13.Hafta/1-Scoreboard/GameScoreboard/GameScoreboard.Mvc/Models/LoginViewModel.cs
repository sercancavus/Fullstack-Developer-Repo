using System.ComponentModel.DataAnnotations;

namespace GameScoreboard.Mvc.Models
{
    public class LoginViewModel
    {
        [Required, MinLength(4), MaxLength(50)]
        public string Nickname { get; set; } = null!;
        [Required, MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = null!;

    }
}
