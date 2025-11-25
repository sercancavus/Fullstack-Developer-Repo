using Microsoft.AspNetCore.Identity;

namespace App.Data.Entities
{
    public class User : IdentityUser
    {
        // Additional profile fields can go here
        public string? DisplayName { get; set; }
        public bool IsSellerRequested { get; set; }
    }
}