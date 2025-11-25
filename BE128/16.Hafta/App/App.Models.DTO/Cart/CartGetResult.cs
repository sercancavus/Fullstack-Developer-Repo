namespace App.Models.DTO.Cart
{
    public class CartGetResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public byte Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = null!;
        public string[] ProductImages { get; set; } = [];
    }
}