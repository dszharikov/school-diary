namespace Grade.DTOs.Input.QuarterlyGrades;

public class CreateQuarterlyGradeDTO
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public string GradeValue { get; set; } = null!;
    public int TermId { get; set; }
}