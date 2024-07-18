using System.ComponentModel.DataAnnotations;

namespace Homework.DTOs;

public class CreateHomeworkDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ClassSubjectId { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Description { get; set; } = null!;
    [Required]
    public DateOnly DueDate { get; set; }  
}