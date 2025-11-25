using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(10)]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}