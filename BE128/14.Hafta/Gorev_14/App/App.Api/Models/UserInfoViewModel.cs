namespace App.Api.Models
{
    public class UserInfoViewModel
    {
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsActive { get; set; }
    }
}