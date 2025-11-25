using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [Required, MinLength(2)]
        public string OrderCode { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(250)]
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Required, Range(1, byte.MaxValue)]
        public byte Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}