namespace Grade.DTOs.Input.AssessmentTypes;

public class CreateAssessmentTypeDTO
{
    public int SubjectId { get; set; }
    public string Name { get; set; } = null!;
}