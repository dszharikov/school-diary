using Microsoft.EntityFrameworkCore;

namespace Term.Data;

public class AppDbContext : DbContext
{
    public DbSet<Models.Term> Terms { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) {}
}