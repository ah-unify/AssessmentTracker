using AssessmentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssessmentTracker.Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public DbSet<Course> Courses { get; protected set; }
    public DbSet<Student> Students { get; protected set; }
}