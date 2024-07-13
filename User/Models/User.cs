using System.ComponentModel.DataAnnotations;

namespace User.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    virtual public string Role { get; set; } = null!;
    public int SchoolId { get; set; }
    public School? School { get; set; }
}