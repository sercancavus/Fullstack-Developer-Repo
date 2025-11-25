using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class ProductCommentViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required, MinLength(2), MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        [Required, Range(1,5)]
        public byte StarCount { get; set; }
    }
}