namespace App.Models.DTO;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public byte? DiscountRate { get; set; }
    public string Description { get; set; } = string.Empty;
    public byte StockAmount { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public ProductReviewDto[] Reviews { get; set; } = [];
}

public class ProductReviewDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public byte StarCount { get; set; }
    public string UserName { get; set; } = string.Empty;
}
