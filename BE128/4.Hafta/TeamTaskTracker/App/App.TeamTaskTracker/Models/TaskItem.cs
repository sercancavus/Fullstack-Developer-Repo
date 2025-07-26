// Models/TaskItem.cs
using System.ComponentModel.DataAnnotations;

namespace TeamTaskTracker.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        [Range(1, 5)]
        public int Priority { get; set; } = 3;
    }
}