namespace Grade.DTOs.Input.TermAssessments;

public class CreateTermAssessmentDTO
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int AssessmentTypeId { get; set; }
    public string GradeValue { get; set; } = null!;
    public int TermId { get; set; }
}