namespace App.Domain;
using System.ComponentModel.DataAnnotations;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int StudentNumber { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [StringLength(20)]
    public string Class { get; set; } = string.Empty;
}
