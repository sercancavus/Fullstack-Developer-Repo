namespace App.Models.DTO;

public class MyOrderDetailResponseDto
{
    public string OrderCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Address { get; set; } = string.Empty;
    public List<MyOrderDetailItemDto> Items { get; set; } = new();
}

public class MyOrderDetailItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public byte Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
