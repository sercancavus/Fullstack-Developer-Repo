namespace App.Api.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? SellerId { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
    }
}
