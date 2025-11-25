using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class ProductEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Kategori seçiniz")] 
        public int CategoryId { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Geçerli bir fiyat giriniz")] 
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string? Details { get; set; }

        [Required]
        [Range(0, 255)] 
        public byte StockAmount { get; set; }

        public bool Enabled { get; set; } = true;
    }
}