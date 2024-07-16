namespace Grade.DTOs.Output;

public class TermAssessmentOutputDTO
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int AssessmentTypeId { get; set; }
    public string GradeValue { get; set; } = null!;
    public int TermId { get; set; }
}