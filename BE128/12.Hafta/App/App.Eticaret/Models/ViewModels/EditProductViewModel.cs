using System.ComponentModel.DataAnnotations;

namespace App.Eticaret.Models.ViewModels
{
    public class EditProductViewModel
    {
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }

        [Required, MinLength(3), MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, Range(0.01, double.MaxValue), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required, Range(1, 255)]
        public byte StockAmount { get; set; }
    }
}
