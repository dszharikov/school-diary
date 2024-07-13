using User.Models;

namespace User.DTOs.Output;

public class SchoolOutputDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;

}