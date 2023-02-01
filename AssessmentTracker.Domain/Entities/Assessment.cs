using AssessmentTracker.Domain.ValueObjects;

namespace AssessmentTracker.Domain.Entities;

public class Assessment : Entity
{
    public string Name { get; set; }
    public Percentage CoursePercentage { get; set; }

    public Guid CourseId { get; protected set; }

    public virtual Course Course { get; protected set; }
    public virtual List<AssessmentRecord> AssessmentRecords { get; protected set; } = new();

    public void BindRecord(AssessmentRecord assessmentRecord)
    {
        AssessmentRecords.Add(assessmentRecord);
    }
}