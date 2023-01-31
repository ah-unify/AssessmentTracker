using AssessmentTracker.Domain.ValueObjects;

namespace AssessmentTracker.Domain.Entities;

public class AssessmentRecord : Entity
{
    public Grade Grade { get; set; }
    
    public Guid StudentId { get; protected set; }
    public Guid AssessmentId { get; protected set; }
    
    public virtual Student Student { get; protected set; }
    public virtual Assessment Assessment { get; protected set; }
}