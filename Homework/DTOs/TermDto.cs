namespace Homework.DTOs;

public class TermDto
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}