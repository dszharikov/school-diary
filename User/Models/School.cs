using System.ComponentModel.DataAnnotations;

namespace User.Models;

public class School
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<User> Users { get; set; } = new();
}