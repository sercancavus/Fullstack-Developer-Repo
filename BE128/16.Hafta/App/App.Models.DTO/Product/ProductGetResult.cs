namespace App.Models.DTO.Product
{
    public class ProductGetResult
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public byte StockAmount { get; set; }
        public bool Enabled { get; set; } = true;
    }
}