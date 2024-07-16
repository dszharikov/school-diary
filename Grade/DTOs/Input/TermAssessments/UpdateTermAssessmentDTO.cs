namespace Grade.DTOs.Input.TermAssessments;

public class UpdateTermAssessmentDTO
{
    public int Id { get; set; }
    public string GradeValue { get; set; } = null!;
}