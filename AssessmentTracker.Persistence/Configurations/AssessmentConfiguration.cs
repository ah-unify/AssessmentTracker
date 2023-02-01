using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssessmentTracker.Persistence.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.Property(t => t.CoursePercentage)
            .HasConversion(x => x.Value, x => new Percentage(x));

        // builder.OwnsOne(t => t.CoursePercentage);
    }
}