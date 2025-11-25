namespace App.Models.DTO;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int? DiscountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public byte StockAmount { get; set; }
}
