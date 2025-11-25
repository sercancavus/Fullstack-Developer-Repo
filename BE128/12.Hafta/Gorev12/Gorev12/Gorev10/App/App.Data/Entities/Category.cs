using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(3), MaxLength(6)]
        public string Color { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(50)]
        public string IconCssClass { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}