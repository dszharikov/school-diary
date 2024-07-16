namespace Grade.DTOs.Input.Grades;

public class CreateGradeDto
{
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public string GradeValue { get; set; } = null!;
    public DateOnly Date { get; set; }
}