using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssessmentTracker.Persistence.Configurations;

public class AssessmentRecordConfiguration : IEntityTypeConfiguration<AssessmentRecord>
{
    public void Configure(EntityTypeBuilder<AssessmentRecord> builder)
    {
        builder.Property(t => t.Grade)
            .HasConversion(x => x.Value, x => new Grade(x));

        // builder.OwnsOne(t => t.Grade);
    }
}