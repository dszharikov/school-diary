using Microsoft.EntityFrameworkCore;
using User.Models;

namespace User.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) {}

    public DbSet<Models.User> Users { get; set; } = null!;
    public DbSet<Parent> Parents { get; set; } = null!;
    public DbSet<School> Schools { get; set; } = null!;
}