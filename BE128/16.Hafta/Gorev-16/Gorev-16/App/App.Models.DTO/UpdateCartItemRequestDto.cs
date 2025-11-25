namespace App.Models.DTO;

public class UpdateCartItemRequestDto
{
    public int CartItemId { get; set; }
    public byte Quantity { get; set; }
}
