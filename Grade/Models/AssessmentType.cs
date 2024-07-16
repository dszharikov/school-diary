namespace Grade.Models;

public class AssessmentType
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string Name { get; set; } = null!;
}