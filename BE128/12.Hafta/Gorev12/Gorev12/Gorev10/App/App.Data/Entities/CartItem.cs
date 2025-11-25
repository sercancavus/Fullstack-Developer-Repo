using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Required, Range(1, byte.MaxValue)]
        public byte Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}