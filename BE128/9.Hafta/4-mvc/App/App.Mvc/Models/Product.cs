using System.ComponentModel.DataAnnotations;

namespace App.Mvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MinLength(3)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required, Range(0,double.MaxValue)]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string? Image { get; set; }
    }
}
