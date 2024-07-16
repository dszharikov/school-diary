using Grade.Models;
using Microsoft.EntityFrameworkCore;

namespace Grade.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) {}

    public DbSet<Models.Grade> Grades { get; set; }
    public DbSet<QuarterlyGrade> QuarterlyGrades { get; set; }
    public DbSet<AssessmentType> AssessmentTypes { get; set; }
    public DbSet<TermAssessment> TermAssessments { get; set; }
}