using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Entities
{
    public class BlogPostEntity : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public long UserId { get; set; }


        // navigation property
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
    }
}
