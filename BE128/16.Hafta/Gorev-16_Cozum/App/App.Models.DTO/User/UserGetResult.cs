namespace App.Models.DTO.User
{
    public class UserGetResult
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Enabled { get; set; }
        public bool HasSellerRequest { get; set; }
    }
}