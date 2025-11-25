namespace App.Models.DTO;

public class CreateCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string IconCssClass { get; set; } = string.Empty;
}
