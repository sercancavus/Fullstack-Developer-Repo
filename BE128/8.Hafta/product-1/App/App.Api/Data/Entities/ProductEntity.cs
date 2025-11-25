using System.ComponentModel.DataAnnotations;

namespace App.Api.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Only Positive")]
        public int Stock { get; set; }
    }
}
