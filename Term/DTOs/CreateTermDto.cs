namespace Term.DTOs;

public class CreateTermDto
{
    public int SchoolId { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}