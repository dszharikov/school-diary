using System.ComponentModel.DataAnnotations;

namespace Homework.Models;

public class Homework
{
    [Key]
    public int Id { get; set; }
    public int ClassSubjectId { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly DueDate { get; set; }
}