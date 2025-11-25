namespace App.Models.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public byte? DiscountPercentage { get; set; }
    public string? ImageUrl { get; set; }
}
