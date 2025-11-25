namespace App.Models.DTO;

public class UpdateCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string IconCssClass { get; set; } = string.Empty;
}
