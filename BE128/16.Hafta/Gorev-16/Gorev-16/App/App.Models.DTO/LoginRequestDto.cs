namespace App.Models.DTO;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string App { get; set; } = string.Empty;
}
