using System.ComponentModel.DataAnnotations;

namespace Homework.DTOs;

public class UpdateHomeworkDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Description { get; set; } = null!;
}