using Microsoft.EntityFrameworkCore;

namespace Homework.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Homework> Homeworks { get; set; } = null!;
}