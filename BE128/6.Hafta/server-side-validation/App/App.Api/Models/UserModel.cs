using System.ComponentModel.DataAnnotations;

namespace App.Api.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Bu alan zorunludur kral!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3-50 arası olmalıydı!")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Range(typeof(DateTime), "1900-01-01" ,"2022-12-31" )]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,24}$")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string PasswordRepeat { get; set; }


    }
}
