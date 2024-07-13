using System.ComponentModel.DataAnnotations;

namespace Term.Models;

public class Term
{
    [Key]
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}