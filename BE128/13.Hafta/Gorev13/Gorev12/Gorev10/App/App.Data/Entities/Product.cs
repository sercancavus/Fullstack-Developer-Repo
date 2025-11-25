using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Geçerli bir fiyat giriniz")]
        public decimal Price { get; set; }
        [MaxLength(1000)]
        public string Details { get; set; } = string.Empty;
        [Required]
        public byte StockAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; set; } = true;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductComment> Comments { get; set; } = new List<ProductComment>();
    }
}