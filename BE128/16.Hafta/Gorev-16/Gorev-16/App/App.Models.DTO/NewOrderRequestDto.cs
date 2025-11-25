namespace App.Models.DTO;

public class NewOrderRequestDto
{
    public List<NewOrderItemDto> Items { get; set; } = new();
    public string DeliveryAddress { get; set; } = string.Empty;
}
