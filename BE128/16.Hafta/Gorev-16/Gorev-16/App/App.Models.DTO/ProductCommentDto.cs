namespace App.Models.DTO;

public class ProductCommentDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public byte StarCount { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
}
