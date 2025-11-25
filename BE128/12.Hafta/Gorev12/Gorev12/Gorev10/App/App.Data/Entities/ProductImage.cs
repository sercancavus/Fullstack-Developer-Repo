using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Required, MinLength(10), MaxLength(250)]
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}