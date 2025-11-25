namespace App.Models.DTO;

public class MyOrdersResponseDto
{
    public List<MyOrderSummaryDto> Orders { get; set; } = new();
}

public class MyOrderSummaryDto
{
    public string OrderCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal TotalPrice { get; set; }
    public int TotalProducts { get; set; }
    public int TotalQuantity { get; set; }
}
