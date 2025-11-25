using Microsoft.AspNetCore.Identity;

namespace App.Api.Data.Entities
{
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
        public bool IsSellerRequested { get; set; }
        public bool IsActive { get; set; } = true;
    }
}