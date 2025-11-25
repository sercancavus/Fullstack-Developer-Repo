using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class ProductComment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [Required, MinLength(2), MaxLength(500)]
        public string Text { get; set; } = string.Empty;
        [Required, Range(1,5)]
        public byte StarCount { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}