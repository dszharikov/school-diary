namespace Grade.DTOs.Input.QuarterlyGrades;

public class UpdateQuarterlyGradeDTO
{
    public int Id { get; set; }
    public string GradeValue { get; set; } = null!;
}